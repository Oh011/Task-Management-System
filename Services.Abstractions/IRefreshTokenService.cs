using Domain.Entities.IdentityModels;
using Shared.Dtos.Identity;

namespace Services.Abstractions
{
    public interface IRefreshTokenService
    {
        public Task<string> CreateRefreshTokenAsync(ApplicationUser user, string deviceId);

        // Generate a new refresh token (this can be a random string or GUID)


        public Task<LogInResultDto> ValidateRefreshTokenAsync(string refreshToken, string deviceId);



        public Task<string> RevokeRefreshToken(string refreshToken, string deviceId);

    }
}
