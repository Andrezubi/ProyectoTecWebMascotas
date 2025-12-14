using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces
{
    public interface ILostPetRepository
    {
        Task<IEnumerable<LostPet>> GetAllLostPetsAsync();
        Task<LostPet> GetLostPetByIdAsync(int id);
        Task InsertLostPetAsync(LostPet lostPet);
        Task UpdateLostPetAsync(LostPet lostPet);
        Task DeleteLostPetAsync(LostPet lostPet);
        Task<IEnumerable<LostPet>> GetPetsByUserId(int userId);
    }
}
