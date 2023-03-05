using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using pets_care.Auth;
using pets_care.Models;
using pets_care.Repository;

namespace pets_care.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetRepository _petRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public PetController(IPetRepository petRepository, ITokenGenerator tokenGenerator)
        {
            _petRepository = petRepository;
            _tokenGenerator = tokenGenerator;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            try
            {
                var pets = await _petRepository.GetPets();
                if (pets == null) return NotFound();

                return Ok(pets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<Pet>> GetPetById(Guid id)
        {
            try
            {
                var pet = await _petRepository.GetPetById(id);
                if (pet == null) return NotFound("Pet not found");

                // GET TOKEN FROM Bearer Token Authorization
                var accessToken = Request.Headers[HeaderNames.Authorization];
                var formatJwt = accessToken.ToString().Replace("Bearer ", "");
                var token = _tokenGenerator.GetName(formatJwt);

                // GET USER ID FROM CLAIM
                var userId = token.Claims.First(c => c.Type == "Id");

                Console.WriteLine("USERID: " + userId);
                Console.WriteLine("PETUSERID: " + pet.ClientId);

                if(!userId.ToString().Contains(pet.ClientId.ToString())) return BadRequest("Wrong Client");

                return Ok(pet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("client/{id}/pets")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPetsByClientId(Guid id)
        {
            try
            {
                var clientPets = await _petRepository.GetPetsByClientId(id);

                if (clientPets == null) return NotFound("Pets not found");

                // GET TOKEN FROM Bearer Token Authorization
                var accessToken = Request.Headers[HeaderNames.Authorization];
                var formatJwt = accessToken.ToString().Replace("Bearer ", "");
                var token = _tokenGenerator.GetName(formatJwt);

                // GET USER ID FROM CLAIM
                var userId = token.Claims.First(c => c.Type == "Id");

                Console.WriteLine("USERID: " + userId);
                Console.WriteLine("PETUSERID: " + id);

                if(!userId.ToString().Contains(id.ToString())) return BadRequest("Wrong Client");

                return Ok(clientPets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}