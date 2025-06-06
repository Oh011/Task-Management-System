using Domain.Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.Dtos.Identity;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    internal class ExternalLoginService(UserManager<ApplicationUser> _userManager,
    ITokenService _tokenService,
    IRefreshTokenService _refreshTokenService
) : IExternalLoginService
    {
        public async Task<LogInResultDto> HandleExternalLogin(ExternalLogInDto dto)
        {

            var email = dto.Email;

            var user = await _userManager.FindByEmailAsync(email);


            if (user == null)
            {


                user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = dto.Name,
                };

                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.Code)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToList());
                    throw new ValidationException();
                }
            }

            var token = await _tokenService.CreateTokenAsync(user);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user, dto.DeviceId);

            return new LogInResultDto
            {

                DisplayName = user.FullName,
                Token = token,
                RefreshToken = refreshToken,
                Email = email

            };

        }
    }
}
