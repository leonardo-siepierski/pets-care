using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Requests
{
    public class PetUpdateRequest
    {
        public string? Longitude {get; set;}

        public string? Latitude {get; set;}
    }
}