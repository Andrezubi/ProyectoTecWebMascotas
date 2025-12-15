using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces.ServiceInterfaces
{
    public interface ILostPetService
    {
        Task<ResponseData> GetAllLostPetsAsync(LostPetQueryFilter filters);
        Task<LostPet> GetLostPetByIdAsync(int id);
        Task InsertLostPetAsync(LostPet lostPet);
        Task UpdateLostPetAsync(LostPet lostPet, string currentEmail, string role);
        Task DeleteLostPetAsync(int petId, string currentEmail, string role);
    }
}
