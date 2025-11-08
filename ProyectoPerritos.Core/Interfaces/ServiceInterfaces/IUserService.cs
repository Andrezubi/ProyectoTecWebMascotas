using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Custom_Entities;
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
        Task<ResponseData> GetAllUsersAsync(UserQueryFilter filters);
        Task<User> GetUserByIdAsync(int id);
        Task InsertUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> loginAsync(string email, string password);
    }
}
