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
        private readonly IUnitOfWork _unitOfWork;
        public UserService( IUnitOfWork unitOfWork) { 

            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {

            var users = await _unitOfWork.UserRepository.GetAll();
            foreach (var user in users) {
                user.Password = "No tienes permiso de ver contrasenias";
            }
            return users;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("No existe usuario con ese id");
            }
            user.Password = "No tienes permiso de ver contrasenias";
            return user;
        }
        public async Task InsertUserAsync(User user)
        {

            var userId = await _unitOfWork.UserRepository.GetById(user.Id);
            if (userId != null)
            {
                throw new Exception("No se pueden repetir los Ids");
            }

            var userCi = await _unitOfWork.UserRepositoryExtra.GetUserByCiAsync(user.Ci);
            if (userCi != null)
            {
                throw new Exception("Este Ci ya no esta disponible");
            }
            var userEmail = await _unitOfWork.UserRepositoryExtra.GetUserByEmailAsync(user.Email);
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
            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();   
        }

        public async Task<bool> loginAsync(string email, string password)
        {
            var user = await _unitOfWork.UserRepositoryExtra.GetUserByEmailAsync(email);
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
            await _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(User user)
        {
            await _unitOfWork.UserRepository.Delete(user.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
