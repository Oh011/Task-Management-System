using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared.Dtos.Identity;

namespace Services
{
    public class RefreshTokenService : IRefreshTokenService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly UserManager<ApplicationUser> _userManager;


        private readonly ITokenService _tokenService;

        private readonly IHttpContextAccessor _httpContextAccessor;




        public RefreshTokenService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ITokenService tokenService, UserManager<ApplicationUser> userManager)
        {

            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _userManager = userManager;
        }
        public async Task<string> CreateRefreshTokenAsync(ApplicationUser user, string deviceId)
        {


            var existingToken = await _unitOfWork.GetRepository<RefreshToken, int>()
                .FindAsync(
                    t => t.UserId == user.Id && t.DeviceId == deviceId && !t.Revoked

                    );


            if (existingToken != null)
            {
                existingToken.Revoked = true;
                _unitOfWork.GetRepository<RefreshToken, int>().Update(existingToken);
                await _unitOfWork.SaveChanges();
            }




            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateRefreshToken(),
                Expiration = DateTime.UtcNow.AddDays(14), // Typically 7 days expiration
                CreatedAt = DateTime.UtcNow,
                Revoked = false,
                DeviceId = deviceId,
                IpAddress = GetIpAddress(),
                UserAgent = GetUserAgent()
            };


            await _unitOfWork.GetRepository<RefreshToken, int>().AddAsync(refreshToken);

            await _unitOfWork.SaveChanges();


            return refreshToken.Token;

        }

        public async Task<string> RevokeRefreshToken(string refreshToken, string deviceId)
        {


            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException("No refresh token found .");



            var repo = _unitOfWork.GetRepository<RefreshToken, int>();

            var Token = await repo
                .FindAsync(
                    t => t.Token == refreshToken && t.DeviceId == deviceId

                    );


            if (Token == null)
            {
                throw new UnauthorizedAccessException("No refresh token found.");
            }


            Token.Revoked = true;

            repo.Update(Token);

            await _unitOfWork.SaveChanges();


            return "Logged out successfully.";


        }

        public async Task<LogInResultDto> ValidateRefreshTokenAsync(string refreshToken, string deviceId)
        {


            var repo = _unitOfWork.GetRepository<RefreshToken, int>();

            var Token = await repo
                .FindAsync(
                    t => t.Token == refreshToken && t.DeviceId == deviceId && !t.Revoked

                    );



            if (Token == null || Token.Expiration < DateTime.UtcNow || Token.Revoked)
            {
                throw new UnauthorizedAccessException("Invalid, expired, or revoked refresh token. Please log in again.");
            }


            var User = await _userManager.FindByIdAsync(Token.UserId);

            if (User == null)
                throw new UnauthorizedAccessException();


            Token.Revoked = true;

            repo.Update(Token);

            await _unitOfWork.SaveChanges();

            var NewRefreshToken = await this.CreateRefreshTokenAsync(User, deviceId);

            var newAccessToken = await _tokenService.CreateTokenAsync(User); // Implement this in



            return new LogInResultDto
            {

                DisplayName = User.FullName,
                Token = newAccessToken,
                RefreshToken = NewRefreshToken,
                Email = User.Email

            };



        }


        //private async Task RevokeAllUserTokens(string userId)
        //{
        //    var repo = _unitOfWork.GetRepository<RefreshToken, int>();

        //    var tokens = await repo.FindAllAsync(t => t.UserId == userId & !t.Revoked);


        //    foreach (var token in userTokens)
        //    {
        //        token.Revoked = true;
        //    }

        //    await _unitOfWork.SaveChanges();
        //}

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32]; // 256-bit token
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        // Get the IP address of the client (Optional)
        private string? GetIpAddress()
        {

            var IpAddress = _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"];

            var RemoteIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();


            if (IpAddress is not null)
                return IpAddress;


            return RemoteIpAddress;


        }

        // Get the User-Agent of the client (Optional)
        private string GetUserAgent()
        {

            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            return userAgent ?? "Unknown"; // Fallback in case the user-agent is not available
        }
    }
}

//The OAuth 2.0 specification recommends that if a refresh token is invalid, expired, or revoked, 
//    the client should prompt the user to log in again.

//This approach ensures that the client application can obtain a new refresh token and access 
//    token after the user reauthenticates.