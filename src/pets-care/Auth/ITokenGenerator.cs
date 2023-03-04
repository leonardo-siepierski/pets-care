using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using pets_care.Models;

namespace pets_care.Auth
{
    public interface ITokenGenerator
    {
        string Generate(Client client);
        ClaimsIdentity AddClaims(Client client);
    }
}