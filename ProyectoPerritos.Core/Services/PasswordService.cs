using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOpt _options;
        public PasswordService(IOptions<PasswordOpt> options)
        {
            _options = options.Value;
        }

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.');
            if (parts.Length != 3)
            {
                throw new FormatException("El formato del Hash no es correcta.");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            byte[] keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                _options.KeySize
            );

            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }

        public string Hash(string password)
        {
            // Generate random salt
            byte[] salt = RandomNumberGenerator.GetBytes(_options.SaltSize);

            // PBKDF2 implementation
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                _options.Iterations,
                HashAlgorithmName.SHA256,
                _options.KeySize
            );

            var keyBase64 = Convert.ToBase64String(key);
            var saltBase64 = Convert.ToBase64String(salt);

            return $"{_options.Iterations}.{saltBase64}.{keyBase64}";
        }
    }

}
