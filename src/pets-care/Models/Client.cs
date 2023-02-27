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

        [MaxLength(20, ErrorMessage = "Adress must have less then 20 digits")]
        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "O email deve ter o formato correto")]
        public string Email { get; set; } = null!;

        [Range(8, 8, ErrorMessage= "Cep must have 8 digits")]
        public int Cep { get; set; }

        [MinLength(6, ErrorMessage = "Adress must have more then 6 digits")]
        public string Adress { get; set; } = null!;

        [Range(6, 15, ErrorMessage = "Password must be between 6 and 15 digits")]
        public string Password { get; set; } = null!;

        
    }
}