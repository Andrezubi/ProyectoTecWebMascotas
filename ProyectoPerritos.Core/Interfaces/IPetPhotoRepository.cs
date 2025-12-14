using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces
{
    public interface IPetPhotoRepository
    {
        Task<IEnumerable<PetPhoto>> GetAllPetPhotosAsync();
        Task<PetPhoto> GetPetPhotoByIdAsync(int id);
        Task InsertPetPhotoAsync(PetPhoto petPhoto, string currentEmail, string role);
        Task UpdatePetPhotoAsync(PetPhoto petPhoto);
        Task DeletePetPhotoAsync(PetPhoto petPhoto);
    }
}
