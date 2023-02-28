using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pets_care.Models;
using pets_care.Requests;

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

        public async void CreateClient(Client client)
        {
            var salt = DateTime.Now.ToString();
            var passwordHashed = HashPassword(client.Password, salt);
            
            var newClient = new Client
            {
                ClientId = Guid.NewGuid(),
                Name = client.Name,
                Email = client.Email,
                Cep = client.Cep,
                Adress = client.Adress,
                Password = passwordHashed,
                CreatedAt = salt,
                ModifiedAt = salt,
            };

            await _context.Clients.AddAsync(newClient);

            _context.SaveChanges();
        }

        public void DeleteClient(Client client)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }

        public void UpdateClient(Client client, ClientRequest clientRequest)
        {
            client.Name = clientRequest.Name;
            client.Email = clientRequest.Email;
            client.ModifiedAt = DateTime.Now.ToString();

            _context.SaveChanges();
        }

        public string HashPassword(string password, string salt)
        {
            SHA256 hash = SHA256.Create();

            var passwordBytes = Encoding.Default.GetBytes($"{password}{salt}");
            var hashedPassword = hash.ComputeHash(passwordBytes);

            return Convert.ToHexString(hashedPassword);
        }

        // public void CheckIfExist()
        // {
        //     var client = await _context.Clients.FindAsync();

        //     return client;
        // }

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