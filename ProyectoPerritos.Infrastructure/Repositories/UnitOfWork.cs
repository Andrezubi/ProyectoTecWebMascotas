using FluentValidation.Validators;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MascotasContext _context;
        public readonly IBaseRepository<User>? _userRepository;
        public readonly IBaseRepository<FoundPet>? _foundPetRepository;
        public readonly IBaseRepository<LostPet>? _lostPetRepository;
        public readonly IBaseRepository<Match>? _matchRepository;
        public readonly IBaseRepository<PetPhoto>? _petPhotoRepository;
        public readonly IUserRepository? _userExtraRepository;
        public UnitOfWork(MascotasContext context)
        {
            _context = context;
        }
        public IBaseRepository<User> UserRepository =>
            _userRepository ?? new BaseRepository<User>(_context);

        public IBaseRepository<FoundPet> FoundPetRepository =>
            _foundPetRepository ?? new BaseRepository<FoundPet>(_context);
        public IBaseRepository<LostPet> LostPetRepository =>
            _lostPetRepository ?? new BaseRepository<LostPet>(_context);
        public IBaseRepository<Match> MatchRepository =>
            _matchRepository ?? new BaseRepository<Match>(_context);
        public IBaseRepository<PetPhoto> PetPhotoRepository =>
            _petPhotoRepository ?? new BaseRepository<PetPhoto>(_context);


        public IUserRepository UserRepositoryExtra =>
            _userExtraRepository ?? new UserRepository(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }




    }
}
