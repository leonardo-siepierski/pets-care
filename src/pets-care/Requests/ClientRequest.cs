using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace pets_care.Requests
{
    public class ClientRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Cep { get; set; } = null!;
        public string Adress { get; set; } = null!;
    }
}