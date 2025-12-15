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
    public interface IFoundPetService
    {
        Task<ResponseData> GetAllFoundPetsAsync(FoundPetQueryFilter filters);
        Task<FoundPet> GetFoundPetByIdAsync(int ? id);
        Task InsertFoundPetAsync(FoundPet foundPet);
        Task UpdateFoundPetAsync(FoundPet foundPet, string currentEmail, string role);
        Task DeleteFoundPetAsync(int petId, string currentEmail, string role);
    }
}
