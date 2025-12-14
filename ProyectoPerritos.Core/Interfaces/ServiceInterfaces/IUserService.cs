using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.QueryFilters;
using SocialMedia.Core.Entities;
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
        Task UpdateUserAsync(User user, string currentEmail, string currentRole);
        Task DeleteUserAsync(int id, string currentEmail, string role);
        Task<bool> loginAsync(string email, string password);

        Task<Posts> GetUserPosts(int id, string currentEmail, string currentRole);
    }
}
