using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pets_care.Auth;
using pets_care.Models;
using pets_care.Repository;
using pets_care.Requests;

namespace pets_care.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITokenGenerator _tokenGenerator;


        public LoginController(IClientRepository clientRepository, ITokenGenerator tokenGenerator)
        {
            _clientRepository = clientRepository;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> ClientLogin([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var client = await _clientRepository.AuthClientAsync(loginRequest);

                if (client == null) return NotFound("Email ou senha inválidos");

                var token = _tokenGenerator.Generate(client);
                
                client.Password = string.Empty;

                return Ok(new { user = client, token});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}