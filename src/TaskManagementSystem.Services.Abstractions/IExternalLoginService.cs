using Shared.Dtos.Identity;

namespace Services.Abstractions
{
    public interface IExternalLoginService
    {

        Task<LogInResultDto> HandleExternalLogin(ExternalLogInDto dto);
    }
}

