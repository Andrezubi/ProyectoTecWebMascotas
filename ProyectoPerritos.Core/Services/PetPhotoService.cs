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
    public class PetPhotoService:IPetPhotoService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public PetPhotoService( IUnitOfWork unitOfWork)
        {
          
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PetPhoto>> GetAllPetPhotosAsync()
        {
            return await _unitOfWork.PetPhotoRepository.GetAll();
        }
        public async Task<PetPhoto> GetPetPhotoByIdAsync(int id)
        {
            return await _unitOfWork.PetPhotoRepository.GetById(id);
        }
        public async Task InsertPetPhotoAsync(PetPhoto petPhoto,string currentEmail, string Role)
        {
            await _unitOfWork.PetPhotoRepository.Add(petPhoto);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdatePetPhotoAsync(PetPhoto petPhoto)
        {
            await _unitOfWork.PetPhotoRepository.Update(petPhoto);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeletePetPhotoAsync(PetPhoto petPhoto)
        {
            await _unitOfWork.PetPhotoRepository.Delete(petPhoto.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
