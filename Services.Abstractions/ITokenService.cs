using Domain.Entities.IdentityModels;

namespace Services.Abstractions
{
    public interface ITokenService
    {

        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}
