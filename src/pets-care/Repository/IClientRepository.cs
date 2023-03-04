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
        Task<Client?> GetClientById(Guid clientId);
        Task<Client?> CreateClient(ClientCreateRequest clientCreateRequest);
        bool UpdateClient(Client client, ClientUpdateRequest clientUpdateRequest);
        void DeleteClient(Client client);
        Task<Client?> AuthClientAsync(LoginRequest loginRequest);
        Task<bool> CheckClientEmail(string email);
  }
}