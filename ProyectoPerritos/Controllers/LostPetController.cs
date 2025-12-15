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
using System.Security.Claims;

namespace ProyectoMascotas.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LostPetController : ControllerBase
    {
        private readonly ILostPetService _lostPetService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public LostPetController(ILostPetService lostPetService, IMapper mapper, IValidationService validationService)
        {
            _lostPetService = lostPetService;
            _mapper = mapper;
            _validationService = validationService;
        }


        /// <summary>
        /// Obtiene todas las mascotas perdidas con paginación y filtros opcionales.
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una lista de las mascotas perdidas paginada según los filtros especificados. 
        /// Se incluye información de paginación en la respuesta. No Filtra Si los filtros se mandan vacios
        /// </remarks>
        /// <param name="filters">Filtros opcionales para la consulta, como página, tamaño de página o criterios de búsqueda.</param>
        /// <returns>Lista paginada de <see cref="LostPetDTO"/> envuelta en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Retorna la lista de mascotas Perdidas correctamente paginada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<LostPetDTO>>))]
        public async Task<IActionResult> GetlostPets([FromQuery] LostPetQueryFilter filters)
        {
            var lostPets = await _lostPetService.GetAllLostPetsAsync(filters);
            var lostPetsDTO = _mapper.Map<IEnumerable<LostPetDTO>>(lostPets.Pagination);


            var pagination = new Pagination
            {
                TotalCount = lostPets.Pagination.TotalCount,
                PageSize = lostPets.Pagination.PageSize,
                CurrentPage = lostPets.Pagination.CurrentPage,
                TotalPages = lostPets.Pagination.TotalPages,
                HasNextPage = lostPets.Pagination.HasNextPage,
                HasPreviousPage = lostPets.Pagination.HasPreviousPage
            };
            var response = new ApiResponse<IEnumerable<LostPetDTO>>(lostPetsDTO)
            {
                Pagination = pagination
            };

            return Ok(response);
        }


        /// <summary>
        /// Obtiene una Mascota Perdida por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint busca una mascota perdida según su identificador único. 
        /// Devuelve un error 400 si la validación del ID falla o 500 si ocurre un error del servidor.
        /// Devuelve error 404 si no encuentra el usuario ocn ID 
        /// </remarks>
        /// <param name="id">Identificador de la mascota a buscar.</param>
        /// <returns>Objeto <see cref="LostPetDTO"/> envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">La mascota fue perdida y retornada correctamente.</response>
        /// <response code="400">El ID de la mascota perdida no es válido.</response>
        /// <response code="404">El La mascota NO fue encontrado</response>
        /// <response code="500">Error interno del servidor.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<LostPetDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetlostPetsById(int id)
        {

            try
            {
                var idRequest = new GetByIdRequest { Id = id };
                var validationResult = await _validationService.ValidateAsync(idRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var lostPets = await _lostPetService.GetLostPetByIdAsync(id);
                var lostPetsDTO = _mapper.Map<LostPetDTO>(lostPets);
                ;
                var response = new ApiResponse<LostPetDTO>(lostPetsDTO);

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
        /// Inserta una nueva mascota perdida.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un <see cref="LostPetDTO"/> y lo guarda en la base de datos. 
        /// Devuelve un error 400 si la validación falla o 500 si ocurre un error del servidor.
        /// </remarks>
        /// <param name="lostPetDTO">Datos de la mascota perdida a crear.</param>
        /// <returns>La Mascota creada envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Mascota Perdida creada exitosamente.</response>
        /// <response code="400">Error de validación de los datos de entrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        /// <response code="401">Error de autorizacion</response>
        [Authorize(Roles = $"{nameof(RoleType.Administrator)},{nameof(RoleType.Consumer)}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<LostPetDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> InsertLostPet([FromBody] LostPetDTO lostPetDTO)
        {
            try
            {
                // La validación automática se hace mediante el filtro
                // Esta validación manual es opcional
                var validationResult = await _validationService.ValidateAsync(lostPetDTO);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }


                var lostPet = _mapper.Map<LostPet>(lostPetDTO);
                await _lostPetService.InsertLostPetAsync(lostPet);

                var response = new ApiResponse<LostPetDTO>(lostPetDTO);

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
        /// Actualiza los datos de la publicacion de una mascota perdida.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un <see cref="LostPetDTO"/> y guarda los cambios necesarios a la mascota perdida y la guarda en  la base de datos. 
        /// Devuelve un error 400 si la validación falla o 500 si ocurre un error del servidor.
        /// </remarks>
        /// <param name="lostPetDTO">Datos de la mascota perdida a actualizar.</param>
        /// <returns>La mascota perdida actualizada envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Mascota Perdida actualizada exitosamente.</response>
        /// <response code="400">Error de validación de los datos de entrada.</response>
        /// <response code="404">Error de datos, no se encontro la mascota perdida</response>
        /// <response code="500">Error interno del servidor.</response>
        /// <response code="401">Error de autenticacion</response>
        [Authorize(Roles = $"{nameof(RoleType.Administrator)},{nameof(RoleType.Consumer)}")]
        [HttpPut]
        [MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<LostPetDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] LostPetDTO lostPetDTO)
        {
            try
            {
                var currentEmail = User.FindFirstValue("Login");
                var currentRole = User.FindFirstValue(ClaimTypes.Role);
                var validationResult = await _validationService.ValidateAsync(lostPetDTO);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                
                var lostPet = _mapper.Map<LostPet>(lostPetDTO);
                await _lostPetService.UpdateLostPetAsync(lostPet, currentEmail, currentRole);

                var response = new ApiResponse<LostPetDTO>(lostPetDTO);

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
        /// Borra una Publicacion de Mascota Perdida por ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint busca una Mascota Perdida según su identificador único y la elimina de la base de datos. 
        /// Devuelve un error 400 si la validación del ID falla o 500 si ocurre un error del servidor.
        /// Devuelve error 404 si no encuentra la mascota perdida 
        /// Devuelve un ok de tipo 204 en caso de que se haya borrado exitosamente
        /// </remarks>
        /// <param name="id">Identificador del usuario a buscar.</param>
        /// <returns>Objeto <see cref="LostPetDTO"/> envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="204">Mascota Perdida encontrada y borrada correctamente.</response>
        /// <response code="400">El ID  no es válido.</response>
        /// <response code="404">Mascota Perdida NO fue encontrada</response>
        /// <response code="500">Error interno del servidor.</response>
        /// <response code="401">Error de falta autorizacion</response>
        [Authorize(Roles = $"{nameof(RoleType.Administrator)},{nameof(RoleType.Consumer)}")]
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(ApiResponse<LostPetDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {

            try
            {
                var currentEmail = User.FindFirstValue("Login");
                var currentRole = User.FindFirstValue(ClaimTypes.Role);
                var idRequest = new GetByIdRequest { Id = id };
                var validationResult = await _validationService.ValidateAsync(idRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                await _lostPetService.DeleteLostPetAsync(id, currentEmail, currentRole);
                return NoContent();

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
