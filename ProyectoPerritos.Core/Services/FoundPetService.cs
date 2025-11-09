using System;
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
    public class FoundPetService:IFoundPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FoundPetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData> GetAllFoundPetsAsync(FoundPetQueryFilter filters)
        {
            var foundPets= await _unitOfWork.FoundPetRepositoryExtra.GetAllFoundPetsAsync();


            //FaltaImplementar los filtros aqui


            var pagedFoundPets = PagedList<object>.Create(foundPets, filters.PageNumber, filters.PageSize);
            if (pagedFoundPets.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de Mascotas Encontradas recuperados correctamente" } },
                    Pagination = pagedFoundPets,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedFoundPets,
                    StatusCode = HttpStatusCode.OK
                };




            }
        }
        public async Task<FoundPet> GetFoundPetByIdAsync(int? id)
        {
            var foundPet = await _unitOfWork.FoundPetRepositoryExtra.GetFoundPetByIdAsync(id);
            if (foundPet == null)
            {
                throw new BusinessException("No existe mascota encontrada con ese id", 404);
            }
            return foundPet;
           
        }
        public async Task InsertFoundPetAsync(FoundPet foundPet)
        {
            var foundPetbyId = await _unitOfWork.FoundPetRepositoryExtra.GetFoundPetByIdAsync(foundPet.Id);
            if (foundPetbyId != null)
            {
                throw new BusinessException("No se pueden repetir los Ids", 400);
            }
            var user = await _unitOfWork.UserRepositoryExtra.GetUserByIdAsync(foundPet.UserId);
            if (user == null)
            {
                throw new BusinessException("NO existe un usuario con ese UserID", 400);
            }
            if (!System.Enum.TryParse<TypeSpecies>(foundPet.Species, true, out var typeSpecies)
    || !System.Enum.IsDefined(typeof(TypeSpecies), typeSpecies))
            {
                throw new BusinessException("Esa Especie de animal no está permitida", 400);
            }
            if (foundPet.DateFound > DateTime.Now)
            {
                throw new BusinessException("La fecha de encuentro Tiene que ser menor a la fecha actual", 400);
            }
            if (ContainsForbiddenWord(foundPet.Description))
            {
                throw new BusinessException("La descripcion no puede contener groserias", 400);
            }



            await _unitOfWork.FoundPetRepository.Add(foundPet);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateFoundPetAsync(FoundPet foundPet)
        {
            await _unitOfWork.FoundPetRepository.Update(foundPet);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteFoundPetAsync(FoundPet foundPet)
        {
            await _unitOfWork.FoundPetRepository.Delete(foundPet.Id);
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
