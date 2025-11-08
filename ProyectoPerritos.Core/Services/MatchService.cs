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
        private readonly IUnitOfWork _unitOfWork;
        public MatchService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _unitOfWork.MatchRepository.GetAll();
        }
        public async Task<Match> GetMatchByIdAsync(int id)
        {

            return await _unitOfWork.MatchRepository.GetById(id);
        }
        public async Task InsertMatchAsync(Match match)
        {
            await _unitOfWork.MatchRepository.Add(match);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateMatchAsync(Match match)
        {
            
            await _unitOfWork.MatchRepository.Update(match);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteMatchAsync(Match match)
        {
            await _unitOfWork.MatchRepository.Delete(match.Id);
            await _unitOfWork.SaveChangesAsync();

        }
    }

}
