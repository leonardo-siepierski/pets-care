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
        // Task<Pet?> GetPetById(Guid clientId);
        // Task<Pet?> CreatePet(ClientCreateRequest clientCreateRequest);
        // bool UpdatePet(Pet client, ClientUpdateRequest clientUpdateRequest);
        // void DeletePet(Pet client);
  }
}