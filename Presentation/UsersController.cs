using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.HelperMethods;
using Services.Abstractions;
using Shared.Dtos;
using Shared.ParameterTypes;

namespace Presentation
{

    [Route("api/users")]

    [Authorize]
    public class UsersController(IServiceManager serviceManager) : ApiController
    {


        [HttpGet]

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetUsers([FromQuery] UserSearchParameters parameters)
        {

            var result = await serviceManager.UserService.GetUsersAsync(parameters);

            return Ok(result);
        }


        [HttpGet("profile")]

        public async Task<ActionResult<UserProfileDto?>> GetUserProfileData()
        {


            var result = await serviceManager.UserService.GetUserProfileAsync(AuthHelpers.GetUserId(HttpContext));


            return Ok(result);
        }



        [HttpPost("upload-image")]

        public async Task<ActionResult<string>> UploadImage(IFormFile file)
        {

            var relativePath = await serviceManager.UserService
                   .UploadProfileImage(file, AuthHelpers.GetUserId(HttpContext));

            var fullUrl = $"{Request.Scheme}://{Request.Host}/{relativePath}";

            return Ok(fullUrl);

        }





    }


}

