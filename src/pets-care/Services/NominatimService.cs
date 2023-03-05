using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using pets_care.Requests;

namespace pets_care.Services
{
    public class NominatimService : INominatimService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://nominatim.openstreetmap.org/reverse?&format=json&lat=-15.12788&lon=-47.10938";

        public NominatimService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<dynamic?> FindAdress(PetUpdateLocationRequest location)
        {
            Console.WriteLine(location);
            Console.WriteLine(location.Longitude);
            Console.WriteLine(location.Latitude);
            // var productValue = new ProductInfoHeaderValue("user_agent", "");
            // _client.DefaultRequestHeaders.UserAgent.Add(productValue);
            var response = await _client.GetStringAsync("");

            // if(!response.IsSuccessStatusCode) return default;

            // var result = await response.ReadAsAsync<object>();
            
            return response;
        }

  }
}