using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Models
{
    [Table("Pet")]
    public class Pet
    {
        [Key]
        public Guid PetId { get; set; }

        [MinLength(2, ErrorMessage = "Name must have more than 2 digits")]
        [Required(ErrorMessage = "Missing field Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Missing field Race")]
        public string Breed { get; set; } = null!;

        [MinLength(3, ErrorMessage= "Cep must have 3 digits")]
        [Required(ErrorMessage = "Missing field Cep")]
        public string Size { get; set; } = null!;

        public string? BirthDay { get; set; }

        [Required(ErrorMessage = "Missing field Age")]
        public int Age { get; set; }

        public string? Longitude {get; set;}

        public string? Latitude {get; set;}

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public Guid ClientId { get; set; }
    }
}