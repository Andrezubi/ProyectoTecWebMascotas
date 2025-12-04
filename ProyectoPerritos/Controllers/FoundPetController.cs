using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Api.Responses;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.Enum;
using ProyectoMascotas.Core.Exceptions;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Core.QueryFilters;
using ProyectoMascotas.Core.Services;
using ProyectoMascotas.Infrastructure.Validators;
using System.Net;

namespace ProyectoMascotas.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FoundPetController : ControllerBase
    {
        private readonly IFoundPetService _foundPetService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public FoundPetController(IFoundPetService foundPetService, IMapper mapper, IValidationService validationService)
        {
            _foundPetService = foundPetService;
            _mapper = mapper;
            _validationService = validationService;
        }


        /// <summary>
        /// Obtiene todas las mascotas encontradas con paginación y filtros opcionales.
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una lista de las mascotas encontradas paginada según los filtros especificados. 
        /// Se incluye información de paginación en la respuesta. No Filtra Si los filtros se mandan vacios
        /// </remarks>
        /// <param name="filters">Filtros opcionales para la consulta, como página, tamaño de página o criterios de búsqueda.</param>
        /// <returns>Lista paginada de <see cref="FoundPetDTO"/> envuelta en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Retorna la lista de mascotas Encontradas correctamente paginada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<FoundPetDTO>>))]
        public async Task<IActionResult> GetfoundPets([FromQuery] FoundPetQueryFilter filters)
        {
            var foundPets = await _foundPetService.GetAllFoundPetsAsync(filters);
            var foundPetsDTO = _mapper.Map<IEnumerable<FoundPetDTO>>(foundPets.Pagination);


            var pagination = new Pagination
            {
                TotalCount = foundPets.Pagination.TotalCount,
                PageSize = foundPets.Pagination.PageSize,
                CurrentPage = foundPets.Pagination.CurrentPage,
                TotalPages = foundPets.Pagination.TotalPages,
                HasNextPage = foundPets.Pagination.HasNextPage,
                HasPreviousPage = foundPets.Pagination.HasPreviousPage
            };
            var response = new ApiResponse<IEnumerable<FoundPetDTO>>(foundPetsDTO)
            {
                Pagination = pagination
            };

            return Ok(response);
        }


        /// <summary>
        /// Obtiene una Mascota Encontrada por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint busca una mascota encontrada según su identificador único. 
        /// Devuelve un error 400 si la validación del ID falla o 500 si ocurre un error del servidor.
        /// Devuelve error 404 si no encuentra el usuario ocn ID 
        /// </remarks>
        /// <param name="id">Identificador de la mascota a buscar.</param>
        /// <returns>Objeto <see cref="FoundPetDTO"/> envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">La mascota fue encontrada y retornada correctamente.</response>
        /// <response code="400">El ID de la mascota encontrada no es válido.</response>
        /// <response code="404">El La mascota NO fue encontrado</response>
        /// <response code="500">Error interno del servidor.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<FoundPetDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetfoundPetsById(int id)
        {

            try
            {
                var idRequest = new GetByIdRequest { Id = id };
                var validationResult = await _validationService.ValidateAsync(idRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var foundPets = await _foundPetService.GetFoundPetByIdAsync(id);
                var foundPetsDTO = _mapper.Map<FoundPetDTO>(foundPets);
                ;
                var response = new ApiResponse<FoundPetDTO>(foundPetsDTO);

                return Ok(response);

            }
            catch (Exception err)
            {
                int statCode = 500;
                if (err is BusinessException)
                {
                    BusinessException? businessException = err as BusinessException;
                    if (businessException != null)
                    {
                        statCode = businessException.StatusCode;
                    }

                }

                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Error", Description = err.Message } },
                    StatusCode = (HttpStatusCode)statCode
                };

                return StatusCode(statCode, responsePost);
            }

        }
        /// <summary>
        /// Inserta una nueva mascota encontrada.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un <see cref="FoundPetDTO"/> y lo guarda en la base de datos. 
        /// Devuelve un error 400 si la validación falla o 500 si ocurre un error del servidor.
        /// </remarks>
        /// <param name="foundPetDTO">Datos de la mascota encontrada a crear.</param>
        /// <returns>La Mascota creada envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Mascota Encontrada creada exitosamente.</response>
        /// <response code="400">Error de validación de los datos de entrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        /// <response code="401">Error de autorizacion</response>
        [Authorize(Roles = $"{nameof(RoleType.Administrator)},{nameof(RoleType.Consumer)}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<FoundPetDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> InsertFoundPet([FromBody] FoundPetDTO foundPetDTO)
        {
            try
            {
                // La validación automática se hace mediante el filtro
                // Esta validación manual es opcional
                var validationResult = await _validationService.ValidateAsync(foundPetDTO);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }


                var foundPet = _mapper.Map<FoundPet>(foundPetDTO);
                await _foundPetService.InsertFoundPetAsync(foundPet);

                var response = new ApiResponse<FoundPetDTO>(foundPetDTO);

                return Ok(response);
            }
            catch (Exception err)
            {
                int statCode = 500;
                if (err is BusinessException)
                {
                    BusinessException? businessException = err as BusinessException;
                    if (businessException != null)
                    {
                        statCode = businessException.StatusCode;
                    }

                }

                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Error", Description = err.Message } },
                    StatusCode = (HttpStatusCode)statCode
                };

                return StatusCode(statCode, responsePost);
            }
        }

    }
}
