using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Exceptions;
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
        public async Task<IEnumerable<PetPhoto>> GetPetPhotoByIdAsync(int PetId, string type)
        {
            if (type.ToLower() == "lost") {
                return await _unitOfWork.PetPhotoRepositoryExtra.GetPetPhotosByPetIdLostPets(PetId);
            }
            else if(type.ToLower() =="found")
            {
                return await _unitOfWork.PetPhotoRepositoryExtra.GetPetPhotosByPetIdFoundPets(PetId);
            }
            else
                throw new BusinessException("Tipo de mascota incorrecto",400) ;
        }
        public async Task InsertPetPhotoAsync(PetPhoto petPhoto,string currentEmail, string Role)
        {
            int? userId;
            if (petPhoto.FoundPetId != null)
            {
                int id=petPhoto.FoundPetId ?? 0;
                var pet = await _unitOfWork.FoundPetRepository.GetById(id);
                userId = pet.UserId;
            }
            else
            {
                int id = petPhoto.LostPetId ?? 0;
                var pet = await _unitOfWork.LostPetRepository.GetById(id);
                userId = pet.UserId;
            }
            var user= await _unitOfWork.UserRepositoryExtra.GetUserByIdAsync(userId);


            if (user.Email != currentEmail && Role != "Administrator")
            {
                throw new BusinessException("El usuario no esta autorizado para actualizar los datos de este perfil", 401);
            }


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
