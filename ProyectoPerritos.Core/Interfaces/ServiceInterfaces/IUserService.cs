using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync(UserQueryFilter filters);
        Task<User> GetUserByIdAsync(int id);
        Task InsertUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> loginAsync(string email, string password);
    }
}
