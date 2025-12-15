using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Exceptions;
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
        public async Task<Match> InsertMatchAsync(int FoundPetId, int LostPetId)
        {
            var FoundPet = await _unitOfWork.FoundPetRepository.GetById(FoundPetId);
            var LostPet =await _unitOfWork.LostPetRepository.GetById(LostPetId);
            if(FoundPet==null || LostPet == null)
            {
                throw new BusinessException("NO se encontro una de las mascotas", 404);
            }

            var similarity = CalculateSimilarity(FoundPet, LostPet);

            Match match = new Match()
            {
                FoundPetId = FoundPetId,
                LostPetId = LostPetId,
                MatchScore = similarity,
                CreatedAt = DateTime.Now,
                Status = "Unresolved"
            };
            await _unitOfWork.MatchRepository.Add(match);
            await _unitOfWork.SaveChangesAsync();
            return match;
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






        public static double CalculateSimilarity(FoundPet found, LostPet lost)
        {
            double score = 0;

            score += Similarity(found.Species, lost.Species) * 0.12;
            score += Similarity(found.Color, lost.Color) * 0.12;
            score += Similarity(found.Breed, lost.Breed) * 0.12;
            score += ExactMatch(found.Sex, lost.Sex) * 0.04;
            score += ExactMatch(found.MicroChip, lost.MicroChip) * 0.20;
            score += DescriptionSimilarity(found.Description, lost.Description) * 0.25;
            score += LocationSimilarity(found.Latitude, found.Longitude,
                                        lost.Latitude, lost.Longitude) * 0.15;

            return Math.Round(score * 100, 2);
        }

        /* ================= STRING ================= */

        private static double Similarity(string? a, string? b)
        {
            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                return 0;

            a = Normalize(a);
            b = Normalize(b);

            int distance = LevenshteinDistance(a, b);
            int maxLen = Math.Max(a.Length, b.Length);

            return maxLen == 0 ? 1 : 1.0 - (double)distance / maxLen;
        }

        private static double ExactMatch(string? a, string? b)
        {
            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                return 0;

            return Normalize(a) == Normalize(b) ? 1 : 0;
        }

        /* ================= DESCRIPTION ================= */

        private static double DescriptionSimilarity(string? a, string? b)
        {
            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
                return 0;

            var tokensA = Tokenize(a);
            var tokensB = Tokenize(b);

            int commonTokens = tokensA.Intersect(tokensB).Count();
            int totalTokens = Math.Max(tokensA.Count, tokensB.Count);

            double tokenScore = totalTokens == 0 ? 0 : (double)commonTokens / totalTokens;
            double levenshteinScore = Similarity(a, b);

            return (tokenScore * 0.6) + (levenshteinScore * 0.4);
        }

        /* ================= LOCATION ================= */

        private static double LocationSimilarity(
            decimal? lat1, decimal? lon1,
            decimal? lat2, decimal? lon2)
        {
            if (!lat1.HasValue || !lon1.HasValue ||
                !lat2.HasValue || !lon2.HasValue)
                return 0;

            double distanceKm = Haversine(
                (double)lat1, (double)lon1,
                (double)lat2, (double)lon2);

            // 0 km => 100%
            // 10 km+ => 0%
            const double maxDistanceKm = 10;

            double score = 1 - Math.Min(distanceKm, maxDistanceKm) / maxDistanceKm;
            return Math.Max(0, score);
        }

        private static double Haversine(
            double lat1, double lon1,
            double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km

            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) *
                Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double DegreesToRadians(double degrees)
            => degrees * Math.PI / 180;

        /* ================= UTIL ================= */

        private static List<string> Tokenize(string text)
        {
            return System.Text.RegularExpressions.Regex.Matches(Normalize(text), @"\b[a-z0-9]{3,}\b")
                .Select(m => m.Value)
                .Distinct()
                .ToList();
        }

        private static string Normalize(string text)
            => text.ToLowerInvariant().Trim();

        /* ================= LEVENSHTEIN ================= */

        private static int LevenshteinDistance(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost);
                }
            }

            return dp[a.Length, b.Length];
        }





    }

}
