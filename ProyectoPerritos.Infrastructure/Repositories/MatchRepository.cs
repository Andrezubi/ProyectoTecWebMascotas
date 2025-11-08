using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoMascotas.Infrastructure.Repositories
{
    public class MatchRepository:IMatchRepository
    {
        private readonly MascotasContext _context;
        public MatchRepository(MascotasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            var matchs = await _context.Matches.ToListAsync();
            
            return matchs;

        }
        public async Task<Match> GetMatchByIdAsync(int id)
        {
            var match = await _context.Matches.FirstOrDefaultAsync(x => x.Id == id);
            return match;
        }
        public async Task InsertMatchAsync(Match match)
        {
            _context.Matches.Add(match);
        }
        public async Task UpdateMatchAsync(Match match)
        {
            _context.Matches.Update(match);

        }
        public async Task DeleteMatchAsync(Match match)
        {
            _context.Matches.Remove(match);

        }
    }
}
