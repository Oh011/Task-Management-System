using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Identity;

namespace Presentation
{


    [Route("api/auth")]
    public class RegistrationController(IServiceManager _serviceManager) : ApiController
    {




        [HttpPost("register")]


        public async Task<ActionResult<RegistrationResponseDto>> Register([FromBody] UserRegisterDto registerDto)
        {


            var result = await _serviceManager.RegistrationService.Register(registerDto);


            return Ok(result);
        }


        [HttpGet("confirm-email")]

        public async Task<ActionResult<string>> confirmEmail([FromQuery] EmailConfirmDto dto)
        {
            var result = await _serviceManager.RegistrationService.ConfirmEmail(dto);


            return Ok(result);

        }
    }
}
