using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Requests
{
    public class PetCreateRequest
    {
        [MinLength(2, ErrorMessage = "Name must have more than 2 digits")]
        [Required(ErrorMessage = "Missing field Name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Missing field Breed")]
        public string Breed { get; set; } = null!;

        [MinLength(3, ErrorMessage= "Size must have 3 digits")]
        [Required(ErrorMessage = "Missing field Size")]
        public string Size { get; set; } = null!;

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Missing field BirthDate")]
        public DateTime BirthDate { get; set; }
    }
}