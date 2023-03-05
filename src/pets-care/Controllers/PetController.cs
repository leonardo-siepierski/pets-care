using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pets_care.Models;
using pets_care.Repository;

namespace pets_care.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetRepository _petRepository;

        public PetController(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetClients()
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
    }
}