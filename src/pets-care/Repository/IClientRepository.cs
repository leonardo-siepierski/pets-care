using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pets_care.Models;

namespace pets_care.Repository
{
    public interface IClientRepository : IDisposable
    {
        Task<IEnumerable<Client>> GetClients();
        Task<Client?> GetClientByID(Guid clientId);
        void InsertClient(Client client);
        void DeleteClient(Client client);
        void UpdateClient(Client client);
        void Save();
    }
}