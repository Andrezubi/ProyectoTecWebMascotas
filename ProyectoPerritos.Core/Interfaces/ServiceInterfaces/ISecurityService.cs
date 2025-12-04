using ProyectoMascotas.Core.Custom_Entities;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces.ServiceInterfaces
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByCredentials(UserLoginRequest userLogin);
        Task RegisterUser(Security security);
    }

}
