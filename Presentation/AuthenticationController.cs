using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Abstractions;
using Shared.Dtos.Identity;

namespace Presentation
{





    [Route("api/auth")]

    public class AuthenticationController(IServiceManager _serviceManager) : ApiController
    {
        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>JWT token and refresh token.</returns>
        /// 
        [HttpPost("login")]
        [EnableRateLimiting("LoginLimiter")]

        public async Task<ActionResult<LogInResultDto>> LogIn(UserLogInDto logInDto)
        {


            var result = await _serviceManager.AuthenticationService.LogIn(logInDto);


            SetRefreshTokenCookie(result.RefreshToken, 14);

            return Ok(new { displayName = result.DisplayName, token = result.Token, email = result.Email });



        }



        [HttpPost("forgot-password")]
        public async Task<ActionResult<string>> ForgetPassword(ForgotPasswordDto dto)
        {

            var result = await _serviceManager.AuthenticationService.ForgetPassword(dto);


            return Ok(result);

        }


        [HttpPost("reset-password")]
        public async Task<ActionResult<string>> ResetPassword(ResetPasswordDto dto)
        {

            var result = await _serviceManager.AuthenticationService.ResetPassword(dto);
            return Ok(result);

        }




        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {

            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _serviceManager.RefreshTokenService.ValidateRefreshTokenAsync(refreshToken, request.DeviceId);


            SetRefreshTokenCookie(result.RefreshToken, 14);



            return Ok(new { displayName = result.DisplayName, token = result.Token, email = result.Email });


        }


        [HttpPost("logout")]

        public async Task<ActionResult<string>> LogOut([FromBody] LogOutRequest request)
        {


            var RefreshToken = Request.Cookies["refreshToken"];


            var result = await _serviceManager.RefreshTokenService.RevokeRefreshToken(RefreshToken, request.DeviceId);


            SetRefreshTokenCookie("", -1);


            return NoContent();




        }

        private void SetRefreshTokenCookie(string refreshToken, int time)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,     // Ensures that the cookie is not accessible via JavaScript (XSS protection)
                Secure = true,       // Ensure the cookie is sent only over HTTPS (use Secure flag)
                Expires = DateTime.UtcNow.AddDays(time),  // Set the expiration date for the cookie
                SameSite = SameSiteMode.Strict, // Controls how cookies are sent with cross-site requests
                Path = "/"           // Cookie is available for the entire application
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }


    }
}


//What does Challenge do?
//Purpose: The Challenge method tells ASP.NET Core to redirect the user to an external provider 
//to perform authentication (in this case, Google).

//Process: When the Challenge method is called, ASP.NET Core initiates the OAuth2.0 authorization 
//flow with the configured provider (Google). It sends the user to Google's authentication page, 
//and Google will handle the login process.