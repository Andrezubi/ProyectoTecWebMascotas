using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
    public class UserRepository:IUserRepository
    {
        private readonly MascotasContext _context;
        private readonly IDapperContext _dapper;
        public UserRepository(MascotasContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM DBMascotas.dbo.Users
                ORDER BY Id",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<User>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public async Task<User> GetUserByIdAsync(int ?id)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM DBMascotas.dbo.Users
                WHERE Id=@Id",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<User>(sql,new {Id=id});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User>GetUserByEmailAsync(string? email)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM DBMascotas.dbo.Users
                WHERE Email=@Email",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<User>GetUserByCiAsync(int ?ci)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => @"
                SELECT *
                FROM DBMascotas.dbo.Users
                WHERE Ci=@Ci",

                    DatabaseProvider.MySql => @"",
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<User>(sql, new { Ci = ci });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task InsertUserAsync(User user)
        {
            _context.Users.Add(user);
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);

        }
        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);

        }

    }
}
