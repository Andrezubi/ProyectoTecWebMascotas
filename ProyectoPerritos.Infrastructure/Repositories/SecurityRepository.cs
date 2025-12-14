using Microsoft.EntityFrameworkCore;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.Interfaces;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(MascotasContext context) : base(context) { }

        public async Task<Security> GetLoginByCredentials(UserLoginRequest login)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Login == login.Email);
        }

        public async Task<Security>GetSecurityByEmailAsync(string email)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Login == email);
        }
    }

    
}
