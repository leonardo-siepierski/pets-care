using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;
using pets_care.Services;

namespace pets_care.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        [Authorize(Roles = "USER")]
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

        [HttpGet]
        [Route("qrcode/{id}")]
        public async Task<ActionResult> GetClientByQRCode(Guid id)
        {
            try
            {
                var client = await _clientRepository.GetClientByID(id);
                if (client == null) return NotFound("Client not found");

                var qrCode = QrCodeService.GenerateByteArray($"https://localhost:7133/client/qrcode/{client.ClientId}");

                return File(qrCode, "image/jpeg");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<string?>> CreateClient([FromBody] ClientCreateRequest clientCreateRequest)
        {
            try
            {
                if (clientCreateRequest == null) return NotFound();

                var clientEmailAlreadyExist = await _clientRepository.CheckClientEmail(clientCreateRequest.Email);
                if(clientEmailAlreadyExist) return BadRequest("Email already in use!");

                var viaCepServiceResponse = await _viaCepService.FindAdress(clientCreateRequest.Cep);
                if (viaCepServiceResponse == null || viaCepServiceResponse.ToString().Contains("erro")) return BadRequest("Cep not found");

                var createdClient = await _clientRepository.CreateClient(clientCreateRequest);

                return CreatedAtAction(nameof(GetClientById), new { id = createdClient?.ClientId}, createdClient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateClient(Guid id, [FromBody] ClientUpdateRequest clientUpdateRequest)
        {
            try
            {
                if (clientUpdateRequest == null) return NotFound();

                var clientFound = await _clientRepository.GetClientByID(id);
                if(clientFound == null) return NotFound("Client not found");

                _clientRepository.UpdateClient(clientFound, clientUpdateRequest);

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