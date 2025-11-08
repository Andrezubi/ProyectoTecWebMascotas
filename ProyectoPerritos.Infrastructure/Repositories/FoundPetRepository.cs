using Microsoft.EntityFrameworkCore;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Repositories
{
    public class FoundPetRepository: IFoundPetRepository
    {
        private readonly MascotasContext _context;
        public FoundPetRepository(MascotasContext context)
        {
            _context= context;
        }

        public async Task<IEnumerable<FoundPet>> GetAllFoundPetsAsync()
        {
            var foundpets = await _context.FoundPets.ToListAsync();
            return foundpets;

        }
        public async Task<FoundPet> GetFoundPetByIdAsync(int id)
        {
            var foundPet = await _context.FoundPets.FirstOrDefaultAsync(x => x.Id == id);
            return foundPet;
        }
        public async Task InsertFoundPetAsync(FoundPet foundPet)
        {
            _context.FoundPets.Add(foundPet);
        }
        public async Task UpdateFoundPetAsync(FoundPet foundPet)
        {
            _context.FoundPets.Update(foundPet);

        }
        public async Task DeleteFoundPetAsync(FoundPet foundPet)
        {
            _context.FoundPets.Remove(foundPet);

        }

    }
    }
