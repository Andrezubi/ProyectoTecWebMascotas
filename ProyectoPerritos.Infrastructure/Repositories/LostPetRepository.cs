using Microsoft.EntityFrameworkCore;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Enum;
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
        private readonly IDapperContext _dapper;
        public LostPetRepository(MascotasContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public async Task<IEnumerable<LostPet>> GetAllLostPetsAsync()
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM LostPet
                ORDER BY Id",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<LostPet>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<LostPet> GetLostPetByIdAsync(int id)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM LostPet
                WHERE Id=@Id
                ORDER BY Id",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<LostPet>(sql, new {Id=id});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task InsertLostPetAsync(LostPet lostPet)
        {
            _context.LostPets.Add(lostPet);
        }
        public async Task UpdateLostPetAsync(LostPet lostPet)
        {
            _context.LostPets.Update(lostPet);

        }
        public async Task DeleteLostPetAsync(LostPet lostPet)
        {
            _context.LostPets.Remove(lostPet);

        }


        public async Task<IEnumerable<LostPet>> GetPetsByUserId(int userId)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM LostPet
                WHERE UserId=@UserId
                ORDER BY Id",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<LostPet>(sql, new { UserId = userId });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
