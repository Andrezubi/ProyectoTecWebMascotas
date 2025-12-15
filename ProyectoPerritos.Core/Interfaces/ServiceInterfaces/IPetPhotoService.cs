using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces.ServiceInterfaces
{
    public interface IPetPhotoService
    {
        Task<IEnumerable<PetPhoto>> GetAllPetPhotosAsync();
        Task<IEnumerable<PetPhoto>> GetPetPhotoByIdAsync(int id,string type);
        Task InsertPetPhotoAsync(PetPhoto petPhoto,string currentEmail, string role);
        Task UpdatePetPhotoAsync(PetPhoto petPhoto);
        Task DeletePetPhotoAsync(PetPhoto petPhoto);
    }
}
