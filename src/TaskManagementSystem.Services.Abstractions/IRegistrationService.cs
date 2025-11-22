using Shared.Dtos.Identity;

namespace Services.Abstractions
{
    public interface IRegistrationService
    {


        Task<RegistrationResponseDto> Register(UserRegisterDto userRegisterDto);



        Task<string> ConfirmEmail(EmailConfirmDto dto);
    }
}
