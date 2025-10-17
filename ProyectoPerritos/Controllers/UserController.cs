using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Api.Responses;
using ProyectoMascotas.Core.Custom_Entities;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Infrastructure.Validators;
using System.Net;
using System.Runtime.CompilerServices;

namespace ProyectoMascotas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public UserController(IUserService userService, IMapper mapper, IValidationService validationService)
        {
            _userService = userService;
            _mapper = mapper;
            _validationService = validationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);

            var response = new ApiResponse<IEnumerable<UserDTO>>(usersDTO);

            return Ok(response);
        }
        [HttpGet("{id}")]
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
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
            

        }

        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody]UserDTO userDTO)
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
                

                var user = _mapper.Map<User>(userDTO);
                await _userService.InsertUserAsync(user);

                var response = new ApiResponse<UserDTO>(userDTO);

                return Ok(response);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if(await _userService.loginAsync(loginRequest.Email, loginRequest.Password))
                {
                    return Ok("login Correcto");
                }
                return BadRequest("Login Incorrecto");

            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }



    }
}
