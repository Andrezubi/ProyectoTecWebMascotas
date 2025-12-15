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
using ProyectoMascotas.Core.Services;
using ProyectoMascotas.Infrastructure.Validators;
using SocialMedia.Core.Entities;
using System.Net;
using System.Security.Claims;

namespace ProyectoMascotas.Api.Controllers
{
    [Authorize]

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PetPhotoController : ControllerBase
    {
        private readonly IPetPhotoService _petPhotoService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public PetPhotoController(IPetPhotoService petPhotoService, IMapper mapper, IValidationService validationService)
        {
            _petPhotoService=petPhotoService;
            _mapper = mapper;
            _validationService = validationService;
        }
        /// <summary>
        /// Inserta una nueva Foto de Mascota.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un <see cref="PetPhotoDTO"/> y lo guarda en la base de datos. 
        /// Devuelve un error 400 si la validación falla o 500 si ocurre un error del servidor.
        /// </remarks>
        /// <param name="petPhotoDTO">Datos de la imagen a crear.</param>
        /// <returns>La imagen de mascota creada envuelta en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Imagen de Mascota creada exitosamente.</response>
        /// <response code="400">Error de validación de los datos de entrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        /// <response code="401">Error de autenticacion</response>
        [Authorize(Roles = $"{nameof(RoleType.Administrator)},{nameof(RoleType.Consumer)}")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<PetPhotoDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> InsertUser([FromBody] PetPhotoDTO petPhotoDTO)
        {
            try
            {
                var currentEmail = User.FindFirstValue("Login");
                var currentRole = User.FindFirstValue(ClaimTypes.Role);

                var validationResult = await _validationService.ValidateAsync(petPhotoDTO);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                var petPhoto = _mapper.Map<PetPhoto>(petPhotoDTO);
                await _petPhotoService.InsertPetPhotoAsync(petPhoto,currentEmail,currentRole);

                var response = new ApiResponse<PetPhotoDTO>(petPhotoDTO);

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
        /// Obtiene las imagenes de una mascota por el id de la mascota.
        /// </summary>
        /// <remarks>
        /// Este endpoint busca fotos de una mascota por el identificador de la mascota. 
        /// Devuelve un error 400 si la validación del ID falla o 500 si ocurre un error del servidor.
        /// Devuelve error 404 si no encuentra el usuario ocn ID 
        /// </remarks>
        /// <param name="id">Identificador de la mascota a buscar.</param>
        /// <param name="type"> Indica si la mascota es una mascota perdida o encontrada puede ser "lost" o "found"</param>
        /// <returns>Objeto <see cref="FoundPetDTO"/> envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">La mascota fue encontrada y retornada correctamente.</response>
        /// <response code="400">El ID de la mascota encontrada no es válido.</response>
        /// <response code="500">Error interno del servidor.</response>
        [AllowAnonymous]
        [HttpGet("{id}/{type}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PetPhoto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetfoundPetsById(int id,string type)
        {

            try
            {
                var idRequest = new GetByIdRequest { Id = id };
                var validationResult = await _validationService.ValidateAsync(idRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var petPhotos = await _petPhotoService.GetPetPhotoByIdAsync(id,type);
                
                ;
                var response = new ApiResponse<IEnumerable<PetPhoto>>(petPhotos);

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
