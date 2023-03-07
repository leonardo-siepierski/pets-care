using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pets_care.Models;

namespace pets_care.Requests
{
    public class ClientResponse
    {
        public Guid ClientId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Cep { get; set; } = null!;

        public string Adress { get; set; } = null!;

        public ClientResponse (Client client)
        {
            ClientId = client.ClientId;
            Name = client.Name;
            Email = client.Email;
            Cep = client.Cep;
            Adress = client.Adress;
        }
    }
}