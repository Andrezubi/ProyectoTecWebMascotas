using ProyectoMascotas.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Interfaces
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match> GetMatchByIdAsync(int id);
        Task InsertMatchAsync(Match match);
        Task UpdateMatchAsync(Match match);
        Task DeleteMatchAsync(Match match);
    }
}
