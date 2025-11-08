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
        private readonly IUnitOfWork _unitOfWork;
        public FoundPetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<FoundPet>> GetAllFoundPetsAsync()
        {
            return await _unitOfWork.FoundPetRepository.GetAll();
        }
        public async Task<FoundPet> GetFoundPetByIdAsync(int id)
        {
            return await _unitOfWork.FoundPetRepository.GetById(id);
        }
        public async Task InsertFoundPetAsync(FoundPet foundPet)
        {
            await _unitOfWork.FoundPetRepository.Add(foundPet);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateFoundPetAsync(FoundPet foundPet)
        {
            await _unitOfWork.FoundPetRepository.Update(foundPet);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteFoundPetAsync(FoundPet foundPet)
        {
            await _unitOfWork.FoundPetRepository.Delete(foundPet.Id);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
