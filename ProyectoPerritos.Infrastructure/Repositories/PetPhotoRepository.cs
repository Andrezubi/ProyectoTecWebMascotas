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
    public class PetPhotoRepository:IPetPhotoRepository
    {
        private readonly MascotasContext _context;
        public PetPhotoRepository(MascotasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PetPhoto>> GetAllPetPhotosAsync()
        {
            var petPhotos = await _context.PetPhotos.ToListAsync();
            return petPhotos;

        }
        public async Task<PetPhoto> GetPetPhotoByIdAsync(int id)
        {
            var petPhoto = await _context.PetPhotos.FirstOrDefaultAsync(x => x.Id == id);
            return petPhoto;
        }
        public async Task InsertPetPhotoAsync(PetPhoto petPhoto)
        {
            _context.PetPhotos.Add(petPhoto);
        }
        public async Task UpdatePetPhotoAsync(PetPhoto petPhoto)
        {
            _context.PetPhotos.Update(petPhoto);

        }
        public async Task DeletePetPhotoAsync(PetPhoto petPhoto)
        {
            _context.PetPhotos.Remove(petPhoto);

        }
    }
}
