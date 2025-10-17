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
    public class LostPetService:ILostPetService
    {
        private readonly ILostPetRepository _lostPetRepository;
        public LostPetService(ILostPetRepository lostPetRepository)
        {
            _lostPetRepository = lostPetRepository;
        }
        public async Task<IEnumerable<LostPet>> GetAllLostPetsAsync()
        {
            return await _lostPetRepository.GetAllLostPetsAsync();
        }
        public async Task<LostPet> GetLostPetByIdAsync(int id)
        {
            return await _lostPetRepository.GetLostPetByIdAsync(id);
        }
        public async Task InsertLostPetAsync(LostPet lostPet)
        {
            await _lostPetRepository.InsertLostPetAsync(lostPet);
        }
        public async Task UpdateLostPetAsync(LostPet lostPet)
        {
            await _lostPetRepository.UpdateLostPetAsync(lostPet);
        }
        public async Task DeleteLostPetAsync(LostPet lostPet)
        {
            await _lostPetRepository.DeleteLostPetAsync(lostPet);

        }
    }
}
