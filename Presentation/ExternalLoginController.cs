using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Identity;
using System.Security.Claims;

namespace Presentation
{

    [Route("api/auth")]
    class ExternalLoginController(IServiceManager _serviceManager) : ApiController
    {


        [HttpGet("external-login")]
        public IActionResult ExternalLogin([FromQuery] string deviceId)
        {

            //Action-Name , ControllerName


            var redirectUrl = Url.Action(
               action: "GoogleResponse",
               controller: "Authentication",  // Make sure this matches your controller's name
               values: new { deviceId = deviceId },  // ✅ deviceId goes here
               protocol: Request.Scheme
                    );


            //302 --> redirect

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "Google");
        }

        //Think of it as "Send the user to Google and come back after login."


        [HttpGet("google-response")]
        public async Task<ActionResult<UserResultDto>> GoogleResponse([FromQuery] string deviceId)
        {



            //retrieves the user's authentication result. 

            var result = await HttpContext.AuthenticateAsync("Google");


            if (!result.Succeeded || result.Principal == null)
                throw new UnAuthorizedException();

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.Identity?.Name;

            var LogInResult = await _serviceManager.ExternalLoginService.HandleExternalLogin(

                new ExternalLogInDto
                {
                    Email = email,
                    Name = name
                }

                );


            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,     // Ensures that the cookie is not accessible via JavaScript (XSS protection)
                Secure = true,       // Ensure the cookie is sent only over HTTPS (use Secure flag)
                Expires = DateTime.UtcNow.AddDays(7),  // Set the expiration date for the cookie
                SameSite = SameSiteMode.Lax, // Controls how cookies are sent with cross-site requests
                Path = "/"           // Cookie is available for the entire application
            };

            Response.Cookies.Append("refreshToken", LogInResult.RefreshToken, cookieOptions);

            return Ok(new { displayName = LogInResult.DisplayName, token = LogInResult.Token, email = LogInResult.Email });
        }
    }
}
