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
using pets_care.Requests;
using pets_care.Services;

namespace pets_care.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetRepository _petRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly INominatimService _nominatimService;

        public PetController(IPetRepository petRepository, ITokenGenerator tokenGenerator, INominatimService nominatimService)
        {
            _petRepository = petRepository;
            _tokenGenerator = tokenGenerator;
            _nominatimService = nominatimService;
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Pet>> CreatePet([FromBody] PetCreateRequest petCreateRequest)
        {
            try
            {
                if (petCreateRequest == null) return NotFound();

                // GET TOKEN FROM Bearer Token Authorization
                var accessToken = Request.Headers[HeaderNames.Authorization];
                var formatJwt = accessToken.ToString().Replace("Bearer ", "");
                var token = _tokenGenerator.GetName(formatJwt);

                // GET USER ID FROM CLAIM
                var clientId = token.Claims.First(c => c.Type == "Id").ToString().Replace("Id: ", "");

                Console.WriteLine("userid:"+ clientId);

                var createdPet = await _petRepository.CreatePet(petCreateRequest, Guid.Parse(clientId));

                return CreatedAtAction(nameof(GetPetById), new { id = createdPet?.PetId}, createdPet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<dynamic>> UpdatePetLocation(Guid id, [FromBody] PetUpdateLocationRequest petUpdateLocationRequest)
        {
            try
            {
                if (petUpdateLocationRequest == null) return NotFound();

                var petFound = await _petRepository.GetPetById(id);
                if(petFound == null) return NotFound("Pet not found");


                var adress = await _nominatimService.FindAdress(petUpdateLocationRequest);
                // if(adress == null) return BadRequest("Invalid Coorinates");

                _petRepository.UpdatePetLocation(petFound, petUpdateLocationRequest);

                return Ok(new {Status = "Pet Location Updated!" , adress});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}