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
    public class ClientController : Controller
    {
        private IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            try
            {
                var clients = await _clientRepository.GetClients();

                if (clients == null) return NotFound();

                return Ok(clients);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(Guid id)
        {
            try
            {
                var client = await _clientRepository.GetClientByID(id);

                if (client == null) return NotFound();

                return Ok(client);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    }
}