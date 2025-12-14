using Microsoft.Extensions.Hosting;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.Exceptions;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Core.QueryFilters;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Claims;
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
        public async Task<ResponseData> GetAllUsersAsync(UserQueryFilter filters)
        {

            var users = await _unitOfWork.UserRepositoryExtra.GetAllUsersAsync();

            if(filters.Name != null)
            {
                users = users.Where(x => x.Name== filters.Name);
            }

           

            var pagedUsers = PagedList<object>.Create(users, filters.PageNumber, filters.PageSize);
            if (pagedUsers.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de Usuarios recuperados correctamente" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedUsers,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepositoryExtra.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new BusinessException("No existe usuario con ese id",404);
            }
            
            return user;
        }
        public async Task InsertUserAsync(User user)
        {

            var userId = await _unitOfWork.UserRepositoryExtra.GetUserByIdAsync(user.Id);
            if (userId != null)
            {
                throw new BusinessException("No se pueden repetir los Ids",400);
            }

            var userCi = await _unitOfWork.UserRepositoryExtra.GetUserByCiAsync(user.Ci);
            if (userCi != null)
            {
                throw new BusinessException("Este Ci ya no esta disponible",400);
            }
            var userEmail = await _unitOfWork.UserRepositoryExtra.GetUserByEmailAsync(user.Email);
            if (userEmail != null)
            {
                throw new BusinessException("Este email ya fue usado", 400);
            }
            if (!user.Password.Any(char.IsUpper))
            {
                throw new BusinessException("La contrasenia debe tener una letra mayuscula", 400);
            }
            if (!user.Password.Any(char.IsLower))
            {
                throw new BusinessException("La contrasenia debe tener una letra minuscula", 400);
            }
            if (!user.Password.Any(char.IsDigit))
            {
                throw new BusinessException("La contrasenia debe contener un numero", 400);
            }
            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();   
        }

        public async Task<bool> loginAsync(string email, string password)
        {
            var user = await _unitOfWork.UserRepositoryExtra.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new BusinessException("No se encontro el email en la base de datos", 404);
            }
            if (user.Password != password)
            {
                throw new BusinessException("Contrasenia incorrecta", 400);
            }
            return true;

        }
        public async Task UpdateUserAsync(User user, string currentEmail, string currentRole)
        {
            var prevUser = await _unitOfWork.UserRepository.GetById(user.Id);

            
            if (prevUser.Email != currentEmail && currentRole != "Administrator")
            {
                throw new BusinessException("El usuario no esta autenticado para ver los posts", 401);
            }
            var prevSecurity = await _unitOfWork.SecurityRepository.GetSecurityByEmailAsync(prevUser.Email);
            
            if (prevSecurity == null)
            {
                throw new BusinessException("El email no se encuentra en la base de datos");
            }
           

            prevSecurity.Name = user.Name;
            prevSecurity.Login = user.Email;
            prevSecurity.Password = user.Password;

            if (prevUser.Email != user.Email)
            {
                var userEmail = await _unitOfWork.UserRepositoryExtra.GetUserByEmailAsync(user.Email);
                if (userEmail != null)
                {
                    throw new BusinessException("Este email ya fue usado", 400);
                }
            }
            if (prevUser.Ci != user.Ci)
            {
                var userCi = await _unitOfWork.UserRepositoryExtra.GetUserByCiAsync(user.Ci);
                if (userCi != null)
                {
                    throw new BusinessException("Este Ci ya fue usado", 400);
                }
            }

            // Update User
            prevUser.Name = user.Name;
            prevUser.Email = user.Email;
            prevUser.Ci = user.Ci;
            prevUser.Phone = user.Phone;
            prevUser.PhotoUrl = user.PhotoUrl;
            prevUser.Password = user.Password;
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(int id, string currentEmail, string role)
        {

            var user = await _unitOfWork.UserRepository.GetById(id);


            if (user == null)
            {
                throw new BusinessException("No existe usuario con ese id", 404);
            }
            if (user.Email != currentEmail && role != "Administrator")
            {
                throw new BusinessException("El usuario no esta autenticado para ver los posts", 401);
            }

            var security = await _unitOfWork.SecurityRepository.GetSecurityByEmailAsync(user.Email);
            await _unitOfWork.SecurityRepository.Delete(security.Id);
            await _unitOfWork.UserRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

        }
        public async Task<Posts> GetUserPosts(int id, string currentEmail, string currentRole) {

            var user = await _unitOfWork.UserRepositoryExtra.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new BusinessException("No existe usuario con ese id", 404);
            }
            if (user.Email != currentEmail && currentRole!="Administrator")
            {
                throw new BusinessException("El usuario no esta autenticado para ver los posts", 401);
            }

            IEnumerable<FoundPet> foundPets= await _unitOfWork.FoundPetRepositoryExtra.GetPetsByUserId(id);
            IEnumerable<LostPet> lostPets = await _unitOfWork.LostPetRepositoryExtra.GetPetsByUserId(id);
            return new Posts
            {
                FoundPets = foundPets,
                LostPets = lostPets
            };
            
        }
    }
}
