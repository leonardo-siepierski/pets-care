using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using pets_care.Requests;

namespace pets_care.Services
{
    public interface INominatimService
    {
        Task<dynamic?> FindAdress(PetUpdateLocationRequest petUpdateLocationRequest);
    }
}