using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Requests
{
    public class ClientViewModel
    {
        public LoginRequest loginRequest { get; set; }
        public string Token { get; set; }
    }
}