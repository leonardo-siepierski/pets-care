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

        public ClientRepository(){}

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

        public async Task<Client?> CreateClient(ClientCreateRequest clientCreateRequest)
        {
            var dateTime = DateTime.Now.ToString();
            var passwordHashed = HashPassword(clientCreateRequest.Password, dateTime);
            
            var newClient = new Client
            {
                ClientId = Guid.NewGuid(),
                Name = clientCreateRequest.Name,
                Email = clientCreateRequest.Email,
                Cep = clientCreateRequest.Cep,
                Adress = clientCreateRequest.Adress,
                Password = passwordHashed,
                Role = "USER",
                CreatedAt = dateTime,
                ModifiedAt = dateTime
            };

            await _context.Clients.AddAsync(newClient);

            _context.SaveChanges();

            newClient.Password = string.Empty;
            newClient.Role = string.Empty;
            newClient.CreatedAt = string.Empty;
            newClient.ModifiedAt = string.Empty;

            return newClient;
        }

        public void DeleteClient(Client client)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }

        public void UpdateClient(Client client, ClientUpdateRequest clientUpdateRequest)
        {
            // update all colunms
            // _context.Entry(client).State = EntityState.Modified;

            // update only the changed colunms
            _context.Clients.Attach(client);

            client.Name = clientUpdateRequest.Name;
            client.Email = clientUpdateRequest.Email;
            client.ModifiedAt = DateTime.Now.ToString();

            _context.SaveChanges();
        }

        public async Task<Client?> AuthClientAsync(LoginRequest loginRequest)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(client => client.Email.Equals(loginRequest.Email));
            if(client == null) return null;
            
            if(client?.Password == HashPassword(loginRequest.Password, client.CreatedAt)) return client;

            return null;
        }



        public string HashPassword(string password, string salt)
        {
            SHA256 hash = SHA256.Create();

            var passwordBytes = Encoding.Default.GetBytes($"{password}{salt}");
            var hashedPassword = hash.ComputeHash(passwordBytes);

            return Convert.ToHexString(hashedPassword);
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