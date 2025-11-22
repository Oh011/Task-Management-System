using Shared;

namespace Services.Abstractions
{
    public interface IEmailService
    {

        Task SendEmailAsync(Email email);
    }
}
