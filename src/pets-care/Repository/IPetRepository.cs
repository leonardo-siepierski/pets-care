using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pets_care.Models;
using pets_care.Requests;

namespace pets_care.Repository
{
    public interface IPetRepository : IDisposable
    {
        Task<IEnumerable<Pet>> GetPets();
        Task<Pet?> GetPetById(Guid petId);
        Task<IEnumerable<Pet>> GetPetsByClientId(Guid clientId);
        Task<Pet?> CreatePet(PetCreateRequest petCreateRequest, Guid clientId);
        void UpdatePetLocation(Pet pet, PetUpdateLocationRequest petUpdateLocationRequest);
        void DeletePet(Pet client);
  }
}