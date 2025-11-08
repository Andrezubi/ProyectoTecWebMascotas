using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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

        public readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction;

        public UnitOfWork(MascotasContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
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
            _userExtraRepository ?? new UserRepository(_context, _dapper);

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _efTransaction?.Dispose();
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

        #region Transacciones
        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                // registrar la conexión/tx en DapperContext
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            // Retornamos la conexión subyacente del DbContext
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }
        #endregion



    }
}
