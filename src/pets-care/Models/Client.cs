using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public Guid ClientId { get; set; }

        [MinLength(6, ErrorMessage = "Name must have more than 6 digits")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Unvalid email format")]
        public string Email { get; set; } = null!;

        [MinLength(8, ErrorMessage= "Cep must have 8 digits")]
        [MaxLength(8, ErrorMessage= "Cep must have 8 digits")]
        public string Cep { get; set; } = null!;

        [MinLength(6, ErrorMessage = "Adress must have more then 6 digits")]
        public string Adress { get; set; } = null!;

        // [Range(6, 15, ErrorMessage = "Password must be between 6 and 15 digits")]
        [MinLength(6, ErrorMessage= "Password must have more than 6 digits")]
        public string Password { get; set; } = null!;

        [DataType("dd/MM/yyyy")]
        public string CreatedAt { get; set; } = null!;

        [DataType("dd/MM/yyyy")]
        public string ModifiedAt { get; set; } = null!;
    }
}