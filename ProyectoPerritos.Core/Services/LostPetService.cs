using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.Enum;
using ProyectoMascotas.Core.Exceptions;
using ProyectoMascotas.Core.Interfaces;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Core.Services
{
    public class LostPetService:ILostPetService
    {

        private readonly IUnitOfWork _unitOfWork;
        public LostPetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ResponseData> GetAllLostPetsAsync(LostPetQueryFilter filters)
        {
            var lostPets = await _unitOfWork.LostPetRepositoryExtra.GetAllLostPetsAsync();


            //FaltaImplementar los filtros aqui


            var pagedLostPets = PagedList<object>.Create(lostPets, filters.PageNumber, filters.PageSize);
            if (pagedLostPets.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de Mascotas Perdidas recuperados correctamente" } },
                    Pagination = pagedLostPets,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedLostPets,
                    StatusCode = HttpStatusCode.OK
                };




            }
        }
        public async Task<LostPet> GetLostPetByIdAsync(int id)
        {
            var lostPet = await _unitOfWork.LostPetRepositoryExtra.GetLostPetByIdAsync(id);
            if (lostPet == null)
            {
                throw new BusinessException("No existe mascota perdida con ese id", 404);
            }
            return lostPet; ;
        }
        public async Task InsertLostPetAsync(LostPet lostPet)
        {
            var lostPetbyId = await _unitOfWork.LostPetRepositoryExtra.GetLostPetByIdAsync(lostPet.Id);
            if (lostPetbyId != null)
            {
                throw new BusinessException("No se pueden repetir los Ids", 400);
            }
            var user = await _unitOfWork.UserRepositoryExtra.GetUserByIdAsync(lostPet.UserId);
            if (user == null)
            {
                throw new BusinessException("NO existe un usuario con ese UserID", 400);
            }
            if (!System.Enum.TryParse<TypeSpecies>(lostPet.Species, true, out var typeSpecies)
    || !System.Enum.IsDefined(typeof(TypeSpecies), typeSpecies))
            {
                throw new BusinessException("Esa Especie de animal no está permitida", 400);
            }
            if (lostPet.DateLost > DateTime.Now)
            {
                throw new BusinessException("La fecha de encuentro Tiene que ser menor a la fecha actual", 400);
            }
            if (ContainsForbiddenWord(lostPet.Description))
            {
                throw new BusinessException("La descripcion no puede contener groserias", 400);
            }



            await _unitOfWork.LostPetRepository.Add(lostPet);
            await _unitOfWork.SaveChangesAsync(); ;
        }
        public async Task UpdateLostPetAsync(LostPet lostPet, string currentEmail, string role)
        {
            var prevPet = await _unitOfWork.LostPetRepository.GetById(lostPet.Id);
            if (prevPet == null)
            {
                throw new BusinessException("No se encontro dicha mascota", 404);
            }
            int userId = prevPet.UserId ?? 0;
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                throw new BusinessException("No se encontro el due;o de la mascota", 404);
            }

            if (user.Email != currentEmail && role != "Administrator")
            {
                throw new BusinessException("El usuario no esta autorizado para actualizar los datos de este perfil", 401);
            }
            prevPet.Species = lostPet.Species;
            prevPet.DateLost = lostPet.DateLost;
            prevPet.Description = lostPet.Description;
            prevPet.Name = lostPet.Name;
            prevPet.Breed = lostPet.Breed;
            prevPet.Latitude = lostPet.Latitude;
            prevPet.Longitude = lostPet.Longitude;
            prevPet.Status = lostPet.Status;
            prevPet.Color = lostPet.Color;
            prevPet.MicroChip = lostPet.MicroChip;
            prevPet.RewardAmount = lostPet.RewardAmount;
            prevPet.Sex = lostPet.Sex;
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteLostPetAsync(int petId, string currentEmail, string role)
        {
            var prevPet = await _unitOfWork.LostPetRepository.GetById(petId);
            if (prevPet == null)
            {
                throw new BusinessException("No se encontro dicha mascota", 404);
            }
            int userId = prevPet.UserId ?? 0;
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                throw new BusinessException("No se encontro el due;o de la mascota", 404);
            }

            if (user.Email != currentEmail && role != "Administrator")
            {
                throw new BusinessException("El usuario no esta autorizado para actualizar los datos de este perfil", 401);
            }
            await _unitOfWork.LostPetRepository.Delete(petId);
            await _unitOfWork.SaveChangesAsync();

        }




        public readonly string[] ForbiddensWords =
        {
            "violencia",
            "odio",
            "groseria",
            "discriminacion",
            "insulto"
        };

        public bool ContainsForbiddenWord(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            foreach (var word in ForbiddensWords)
            {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
