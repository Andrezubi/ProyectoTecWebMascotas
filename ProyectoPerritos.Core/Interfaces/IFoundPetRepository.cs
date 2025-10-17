using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces
{
    public interface IFoundPetRepository
    {
        Task<IEnumerable<FoundPet>> GetAllFoundPetsAsync();
        Task<FoundPet> GetFoundPetByIdAsync(int id);
        Task InsertFoundPetAsync(FoundPet foundPet);
        Task UpdateFoundPetAsync(FoundPet foundPet);
        Task DeleteFoundPetAsync(FoundPet foundPet);
    }
}

