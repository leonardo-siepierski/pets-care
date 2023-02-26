using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pets_care.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://viacep.com.br/ws/";

        public ViaCepService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<object?> FindAdress(string cep)
        {
            var response = await _client.GetAsync($"{cep}/json");

            if(!response.IsSuccessStatusCode) return default;

            var result = await response.Content.ReadFromJsonAsync<object>();
            
            return result;
        }
    }
}