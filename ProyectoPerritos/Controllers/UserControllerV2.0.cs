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
using ProyectoMascotas.Infrastructure.Validators;
using SocialMedia.Core.Entities;
using System.Net;

namespace ProyectoMascotas.Api.Controllers
{
    [Authorize]
    
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserControllerV2 : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;
        public UserControllerV2(IUserService userService, IMapper mapper, IValidationService validationService, ISecurityService securityService, IPasswordService passwordService)
        {
            _userService = userService;
            _mapper = mapper;
            _validationService = validationService;
            _securityService = securityService;
            _passwordService = passwordService;
        }




        /// <summary>
        /// Obtiene todos los usuarios con paginación y filtros opcionales.
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una lista de usuarios paginada según los filtros especificados. 
        /// Se incluye información de paginación en la respuesta. No Filtra Si los filtros se mandan vacios
        /// </remarks>
        /// <param name="filters">Filtros opcionales para la consulta, como página, tamaño de página o criterios de búsqueda.</param>
        /// <returns>Lista paginada de <see cref="UserDTO"/> envuelta en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Retorna la lista de usuarios correctamente paginada.</response>
        /// <response code="401">Error de falta autorizacion</response>
        /// <response code="500">Error interno de servidor</response>
        [Authorize(Roles = nameof(RoleType.Administrator))]

        [HttpGet]
        [MapToApiVersion("2.0")]

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDTO>>))]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryFilter filters)
        {
            var users = await _userService.GetAllUsersAsync(filters);
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users.Pagination);


            var pagination = new Pagination
            {
                TotalCount = users.Pagination.TotalCount,
                PageSize = users.Pagination.PageSize,
                CurrentPage = users.Pagination.CurrentPage,
                TotalPages = users.Pagination.TotalPages,
                HasNextPage = users.Pagination.HasNextPage,
                HasPreviousPage = users.Pagination.HasPreviousPage
            };
            var response = new ApiResponse<IEnumerable<UserDTO>>(usersDTO)
            {
                Pagination = pagination
            };

            return Ok(response);
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint busca un usuario según su identificador único. 
        /// Devuelve un error 400 si la validación del ID falla o 500 si ocurre un error del servidor.
        /// Devuelve error 404 si no encuentra el usuario ocn ID 
        /// </remarks>
        /// <param name="id">Identificador del usuario a buscar.</param>
        /// <returns>Objeto <see cref="UserDTO"/> envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Usuario encontrado y retornado correctamente.</response>
        /// <response code="400">El ID del usuario no es válido.</response>
        /// <response code="404">El Usuario NO fue encontrado</response>
        /// <response code="500">Error interno del servidor.</response>
        /// <response code="401">Error de falta autorizacion</response>
        [Authorize(Roles = nameof(RoleType.Administrator))]
        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {

            try
            {
                var idRequest = new GetByIdRequest { Id = id };
                var validationResult = await _validationService.ValidateAsync(idRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var user = await _userService.GetUserByIdAsync(id);
                var userDTO = _mapper.Map<UserDTO>(user);
                ;
                var response = new ApiResponse<UserDTO>(userDTO);

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
        /// Inserta un nuevo usuario.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe un <see cref="UserDTO"/> y lo guarda en la base de datos. 
        /// Devuelve un error 400 si la validación falla o 500 si ocurre un error del servidor.
        /// </remarks>
        /// <param name="userDTO">Datos del usuario a crear.</param>
        /// <returns>El usuario creado envuelto en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Usuario creado exitosamente.</response>
        /// <response code="400">Error de validación de los datos de entrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        [AllowAnonymous]
        [HttpPost]
        [MapToApiVersion("2.0")]

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> InsertUser([FromBody] UserDTO userDTO)
        {
            try
            {
                // La validación automática se hace mediante el filtro
                // Esta validación manual es opcional
                var validationResult = await _validationService.ValidateAsync(userDTO);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }


                var security = _mapper.Map<Security>(userDTO);
                security.Password = _passwordService.Hash(security.Password);
                await _securityService.RegisterUser(security);

                userDTO.Password = security.Password;
                var user = _mapper.Map<User>(userDTO);
                await _userService.InsertUserAsync(user);

                var response = new ApiResponse<UserDTO>(userDTO);

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
