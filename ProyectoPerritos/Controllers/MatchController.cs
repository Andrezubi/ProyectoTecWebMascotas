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
using System.Net;
using System.Security.Claims;

namespace ProyectoMascotas.Api.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public MatchController(IMatchService matchService, IMapper mapper, IValidationService validationService)
        {
            _matchService = matchService;
            _mapper = mapper;
            _validationService = validationService;
        }
        /// <summary>
        /// Se inserta los ids de una mascota encontrada y una perdida y se los compara.
        /// </summary>
        /// <remarks>
        /// Este endpoint recibe dos <see cref="int"/> y lo guarda en la base de datos. 
        /// Devuelve un error 400 si la validación falla o 500 si ocurre un error del servidor.
        /// </remarks>
        /// <param name="lostPetId">Id de la Mascota Perdida</param>
        /// <param name="foundPetId">Id de la Mascota Encontrada</param>
        /// <returns>La Match creada envuelta en <see cref="ApiResponse{T}"/>.</returns>
        /// <response code="200">Comparacion de mascotas creada exitosamente.</response>
        /// <response code="400">Error de validación de los datos de entrada.</response>
        /// <response code="500">Error interno del servidor.</response>
        

        [HttpPost("{foundPetId}/{lostPetId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<PetPhotoDTO>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        
        public async Task<IActionResult> InsertUser(int foundPetId,int lostPetId)
        {
            try
            {

                var idRequest = new GetByIdRequest { Id = foundPetId };
                var validationResult = await _validationService.ValidateAsync(idRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                idRequest = new GetByIdRequest { Id = lostPetId };
                validationResult = await _validationService.ValidateAsync(idRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }

                var match =await  _matchService.InsertMatchAsync(foundPetId, lostPetId);

                var matchDTO = _mapper.Map<MatchDTO>(match);
                
                var response = new ApiResponse<MatchDTO>(matchDTO);

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
