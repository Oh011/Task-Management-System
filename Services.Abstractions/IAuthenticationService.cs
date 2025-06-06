using Shared.Dtos.Identity;

namespace Services.Abstractions
{
    public interface IAuthenticationService
    {

        Task<LogInResultDto> LogIn(UserLogInDto Dto);

        Task<string> ForgetPassword(ForgotPasswordDto dto);


        Task<string> ResetPassword(ResetPasswordDto dto);
    }
}
