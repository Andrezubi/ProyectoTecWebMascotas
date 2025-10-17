using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Services
{
    public class FoundPetService:IFoundPetService
    {
        private readonly IFoundPetRepository _foundPetRepository;
        public FoundPetService(IFoundPetRepository foundPetRepository)
        {
            _foundPetRepository = foundPetRepository;
        }
        public async Task<IEnumerable<FoundPet>> GetAllFoundPetsAsync()
        {
            return await _foundPetRepository.GetAllFoundPetsAsync();
        }
        public async Task<FoundPet> GetFoundPetByIdAsync(int id)
        {
            return await _foundPetRepository.GetFoundPetByIdAsync(id);
        }
        public async Task InsertFoundPetAsync(FoundPet foundPet)
        {
            await _foundPetRepository.InsertFoundPetAsync(foundPet);
        }
        public async Task UpdateFoundPetAsync(FoundPet foundPet)
        {
            await _foundPetRepository.UpdateFoundPetAsync(foundPet);
        }
        public async Task DeleteFoundPetAsync(FoundPet foundPet)
        {
            await _foundPetRepository.DeleteFoundPetAsync(foundPet);

        }
    }
}
