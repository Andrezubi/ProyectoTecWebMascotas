using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoMascotas.Api.Data;
using ProyectoMascotas.Api.Responses;
using ProyectoMascotas.Core.Interfaces.ServiceInterfaces;
using ProyectoMascotas.Infrastructure.Validators;
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
        public UserController(IUserService userService,IMapper mapper, IValidationService validationService)
        {
            _userService = userService;
            _mapper = mapper;
            _validationService = validationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);

            var response = new ApiResponse<IEnumerable<UserDTO>>(usersDTO);

            return Ok(response);
        }




    }
}
