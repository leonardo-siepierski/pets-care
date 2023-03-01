using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeBankAuth.Services;
using Microsoft.AspNetCore.Mvc;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;

namespace pets_care.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IClientRepository _clientRepository;


        public LoginController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> ClientLogin([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var client = await _clientRepository.AuthClientAsync(loginRequest);

                if (client == null) return NotFound("Email ou senha inválidos");

                var token = new TokenGenerator().Generate(client);
                // client.Password = string.Empty;

                return Ok(new { user = client, token});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}