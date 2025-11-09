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
    public class FoundPetRepository: IFoundPetRepository
    {
        private readonly MascotasContext _context;
        private readonly IDapperContext _dapper;
        public FoundPetRepository(MascotasContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public async Task<IEnumerable<FoundPet>> GetAllFoundPetsAsync()
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM FoundPet
                ORDER BY Id",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<FoundPet>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            ;

        }
        public async Task<FoundPet> GetFoundPetByIdAsync(int ?id)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM FoundPet
                WHERE Id=@Id
                ORDER BY Id",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<FoundPet>(sql, new {Id=id});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
