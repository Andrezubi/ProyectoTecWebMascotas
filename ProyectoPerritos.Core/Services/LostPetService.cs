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

        private readonly IUnitOfWork _unitOfWork;
        public LostPetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<LostPet>> GetAllLostPetsAsync()
        {
            return await _unitOfWork.LostPetRepository.GetAll();
        }
        public async Task<LostPet> GetLostPetByIdAsync(int id)
        {
            return await _unitOfWork.LostPetRepository.GetById(id);
        }
        public async Task InsertLostPetAsync(LostPet lostPet)
        {
            await _unitOfWork.LostPetRepository.Add(lostPet);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateLostPetAsync(LostPet lostPet)
        {
            await _unitOfWork.LostPetRepository.Update(lostPet);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteLostPetAsync(LostPet lostPet)
        {
            await _unitOfWork.LostPetRepository.Delete(lostPet.Id);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
