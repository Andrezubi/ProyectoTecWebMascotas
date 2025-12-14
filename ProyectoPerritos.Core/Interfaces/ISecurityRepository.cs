using ProyectoMascotas.Core.Custom_Entities;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces
{
    public interface ISecurityRepository : IBaseRepository<Security>
    {
        Task<Security> GetLoginByCredentials(UserLoginRequest login);
        Task<Security> GetSecurityByEmailAsync(string email);
    }
}
