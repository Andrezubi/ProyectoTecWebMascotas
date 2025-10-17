using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) { 
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users= await _userRepository.GetAllUsersAsync();
            foreach (var user in users) {
                user.Password = "No tienes permiso de ver contrasenias";
            }
            return users;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new Exception("No existe usuario con ese id");
            }
            user.Password = "No tienes permiso de ver contrasenias";
            return user;
        }
        public async Task InsertUserAsync(User user)
        {

            var userId = await _userRepository.GetUserByIdAsync(user.Id);
            if (userId != null)
            {
                throw new Exception("No se pueden repetir los Ids");
            }

            var userCi = await _userRepository.GetUserByCiAsync(user.Ci);
            if (userCi != null)
            {
                throw new Exception("Este Ci ya no esta disponible");
            }
            var userEmail = await _userRepository.GetUserByEmailAsync(user.Email);
            if (userEmail != null)
            {
                throw new Exception("Este email ya fue usado");
            }
            if (!user.Password.Any(char.IsUpper))
            {
                throw new Exception("La contrasenia debe tener una letra mayuscula");
            }
            if (!user.Password.Any(char.IsLower))
            {
                throw new Exception("La contrasenia debe tener una letra minuscula");
            }
            if (!user.Password.Any(char.IsDigit))
            {
                throw new Exception("La contrasenia debe contener un numero");
            }
            await _userRepository.InsertUserAsync(user);
        }

        public async Task<bool> loginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("No se encontro el email en la base de datos");
            }
            if (user.Password != password)
            {
                throw new Exception("Contrasenia incorrecta");
            }
            return true;

        }
        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }
        public async Task DeleteUserAsync(User user)
        {
            await _userRepository.DeleteUserAsync(user);

        }
    }
}
