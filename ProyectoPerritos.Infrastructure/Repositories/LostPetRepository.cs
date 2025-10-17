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
    public class LostPetRepository:ILostPetRepository
    {
        private readonly MascotasContext _context;
        public LostPetRepository(MascotasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LostPet>> GetAllLostPetsAsync()
        {
            var lostPets = await _context.LostPets.ToListAsync();
            return lostPets;

        }
        public async Task<LostPet> GetLostPetByIdAsync(int id)
        {
            var lostPet = await _context.LostPets.FirstOrDefaultAsync(x => x.Id == id);
            return lostPet;
        }
        public async Task InsertLostPetAsync(LostPet lostPet)
        {
            _context.LostPets.Add(lostPet);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateLostPetAsync(LostPet lostPet)
        {
            _context.LostPets.Update(lostPet);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteLostPetAsync(LostPet lostPet)
        {
            _context.LostPets.Remove(lostPet);
            await _context.SaveChangesAsync();

        }
    }
}
