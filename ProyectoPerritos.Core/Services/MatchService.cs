using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Services
{
    public class MatchService:IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }
        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _matchRepository.GetAllMatchesAsync();
        }
        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _matchRepository.GetMatchByIdAsync(id);
        }
        public async Task InsertMatchAsync(Match match)
        {
            await _matchRepository.InsertMatchAsync(match);
        }
        public async Task UpdateMatchAsync(Match match)
        {
            await _matchRepository.UpdateMatchAsync(match);
        }
        public async Task DeleteMatchAsync(Match match)
        {
            await _matchRepository.DeleteMatchAsync(match);

        }
    }

}
