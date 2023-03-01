using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Requests
{
    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}