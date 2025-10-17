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
        private readonly IPetPhotoRepository _petPhotoRepository;
        public PetPhotoService(IPetPhotoRepository petPhotoRepository)
        {
            _petPhotoRepository = petPhotoRepository;
        }
        public async Task<IEnumerable<PetPhoto>> GetAllPetPhotosAsync()
        {
            return await _petPhotoRepository.GetAllPetPhotosAsync();
        }
        public async Task<PetPhoto> GetPetPhotoByIdAsync(int id)
        {
            return await _petPhotoRepository.GetPetPhotoByIdAsync(id);
        }
        public async Task InsertPetPhotoAsync(PetPhoto petPhoto)
        {
            await _petPhotoRepository.InsertPetPhotoAsync(petPhoto);
        }
        public async Task UpdatePetPhotoAsync(PetPhoto petPhoto)
        {
            await _petPhotoRepository.UpdatePetPhotoAsync(petPhoto);
        }
        public async Task DeletePetPhotoAsync(PetPhoto petPhoto)
        {
            await _petPhotoRepository.DeletePetPhotoAsync(petPhoto);

        }
    }
}
