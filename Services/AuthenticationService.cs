using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared;
using Shared.Dtos.Identity;

namespace Services
{
    class AuthenticationService(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, ILinkBuilder linkBuilder, IEmailService emailService, IRefreshTokenService _refreshTokenService) : IAuthenticationService
    {
        public async Task<string> ForgetPassword(ForgotPasswordDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email);


            if (user == null)
            {
                throw new NotFoundException($"No user found with email: {dto.Email}");

            }


            var EmailIsConfirmed = await _userManager.IsEmailConfirmedAsync(user);



            if (!EmailIsConfirmed)
            {

                throw new ForbiddenException("Email is not confirmed. Please confirm your email before logging in.");
            }


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var ResetLink = linkBuilder.BuildPasswordResetLink(token, user.Email);

            var email = new Email
            {
                To = dto.Email,
                Subject = "Password Reset",
                Body = $"Reset your password using this link: {ResetLink}"
            };


            await emailService.SendEmailAsync(email);
            return "Password reset email sent successfully.";

        }

        public async Task<LogInResultDto> LogIn(UserLogInDto Dto)
        {


            var user = await _userManager.FindByEmailAsync(Dto.Email);

            if (user == null)
            {


                throw new UnAuthorizedException("Invalid credentials.");
            }

            var EmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!EmailConfirmed)
                throw new ForbiddenException("Email is not confirmed");

            // Check if user is locked out
            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new AccountLockedException();
            }



            var passwordValid = await _userManager.CheckPasswordAsync(user, Dto.Password);

            if (!passwordValid)
            {
                // Increment failed login count
                await _userManager.AccessFailedAsync(user);

                // Check if user is locked out now
                if (await _userManager.IsLockedOutAsync(user))
                {
                    throw new AccountLockedException();
                }

                throw new UnAuthorizedException("Invalid credentials.");
            }






            var Token = await tokenService.CreateTokenAsync(user);
            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user, Dto.DeviceId);


            return new LogInResultDto
            {

                DisplayName = user.FullName,
                Token = Token,
                RefreshToken = refreshToken,
                Email = Dto.Email

            };


        }

        public async Task<string> ResetPassword(ResetPasswordDto dto)
        {


            var user = await _userManager.FindByEmailAsync(dto.Email);


            if (user == null)
                throw new NotFoundException("Invalid email.");




            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);


            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToList());

                throw new ValidationException(errors);
            }


            return "Password has been reset successfully.";
        }
    }
}
