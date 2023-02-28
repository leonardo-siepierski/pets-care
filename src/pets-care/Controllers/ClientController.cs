using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;
using pets_care.Services;

namespace pets_care.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IViaCepService _viaCepService;

        public ClientController(IClientRepository clientRepository, IViaCepService viaCepService)
        {
            _clientRepository = clientRepository;
            _viaCepService = viaCepService;
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
            catch (Exception ex)
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
                if (client == null) return NotFound("Client not found");

                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<string?>> CreateClient([FromBody] Client client)
        {
            try
            {
                if (client == null) return NotFound();

                var viaCepServiceResponse = await _viaCepService.FindAdress(client.Cep);
                if (viaCepServiceResponse == null || viaCepServiceResponse.ToString().Contains("erro")) return BadRequest("Cep not found");

                var createdClient = await _clientRepository.CreateClient(client);

                return CreatedAtAction(nameof(GetClientById), new { id = createdClient?.ClientId}, createdClient);
            }
            catch (Exception ex)
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
                if(clientFound == null) return NotFound("Client not found");

                _clientRepository.UpdateClient(clientFound, clientRequest);

                return Ok("Client Updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteClient(Guid id)
        {
            try
            {
                var clientFound = await _clientRepository.GetClientByID(id);
                if(clientFound == null) return NotFound("Client not found");

                _clientRepository.DeleteClient(clientFound);

                return Ok("Client Deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    }
}