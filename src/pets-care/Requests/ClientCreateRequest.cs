using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Requests
{
    public class ClientCreateRequest
    {
        [MinLength(6, ErrorMessage = "Name must have more than 6 digits")]
        [Required(ErrorMessage = "Missing field Name")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Unvalid email format")]
        [Required(ErrorMessage = "Missing field Email")]
        public string Email { get; set; } = null!;

        [MinLength(8, ErrorMessage= "Cep must have 8 digits")]
        [MaxLength(8, ErrorMessage= "Cep must have 8 digits")]
        [Required(ErrorMessage = "Missing field Cep")]
        public string Cep { get; set; } = null!;

        [MinLength(6, ErrorMessage = "Adress must have more then 6 digits")]
        [Required(ErrorMessage = "Missing field Adress")]
        public string Adress { get; set; } = null!;

        [MinLength(6, ErrorMessage= "Password must have more than 6 digits")]
        [Required(ErrorMessage = "Missing field Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}