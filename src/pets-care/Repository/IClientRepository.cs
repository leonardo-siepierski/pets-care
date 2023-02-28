using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pets_care.Models;
using pets_care.Requests;

namespace pets_care.Repository
{
    public interface IClientRepository : IDisposable
    {
        Task<IEnumerable<Client>> GetClients();
        Task<Client?> GetClientByID(Guid clientId);
        void CreateClient(Client client);
        void UpdateClient(Client client, ClientRequest clientRequest);
        void DeleteClient(Client client);
    }
}