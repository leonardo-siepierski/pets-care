using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;

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

        [HttpPost]
        public ActionResult<string> CreateClient([FromBody] Client client)
        {
            try
            {
                if (client == null) return NotFound();

                _clientRepository.CreateClient(client);

                return Ok($"Created!");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateClient(Guid id, [FromBody] ClientRequest clientRequest)
        {
            try
            {
                if (clientRequest == null) return NotFound();

                var clientFound = await _clientRepository.GetClientByID(id);
                if(clientFound == null) return NotFound();

                _clientRepository.UpdateClient(clientFound, clientRequest);

                return Ok($"Updated!");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    }
}