using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces.ServiceInterfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match> GetMatchByIdAsync(int id);
        Task<Match> InsertMatchAsync(int FoundPetId, int LostPetId);
        Task UpdateMatchAsync(Match match);
        Task DeleteMatchAsync(Match match);
    }
}
