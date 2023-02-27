using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pets_care.Models;

namespace pets_care.Repository
{
    public class ClientRepository : IClientRepository, IDisposable
    {
        protected readonly PetCareContext _context;
        
        public ClientRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();

            return clients;
        }

        public async Task<Client?> GetClientByID(Guid clientId)
        {
            var client = await _context.Clients.FindAsync(clientId);

            return client;
        }

        public async void InsertClient(Client client)
        {
            await _context.Clients.AddAsync(client);
        }

        public void DeleteClient(Client client)
        {
            _context.Clients.Remove(client);
        }

        public void UpdateClient(Client client)
        {
            _context.Entry(client).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}