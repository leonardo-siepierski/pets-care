using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace pets_care.Requests
{
    public class ClientUpdateRequest
    {
        [MinLength(6, ErrorMessage = "Name must have more than 6 digits")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Unvalid email format")]
        public string Email { get; set; } = null!;
    }
}