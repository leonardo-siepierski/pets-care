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
    public class PetRepository : IPetRepository, IDisposable
    {
        protected readonly PetCareContext _context;
        
        public PetRepository(PetCareContext context)
        {
            _context = context;
        }

        public PetRepository()
        {
        }

        public async Task<IEnumerable<Pet>> GetPets()
        {
            var pets = await _context.Pets.ToListAsync();

            return pets;
        }

        public async Task<Pet?> GetPetById(Guid petId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            
            return pet;
        }

        public async Task<IEnumerable<Pet>> GetPetsByClientId(Guid clientId)
        {
            var clientPets = await _context.Pets.Where(pet => pet.ClientId == clientId).ToListAsync();
            
            return clientPets;
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