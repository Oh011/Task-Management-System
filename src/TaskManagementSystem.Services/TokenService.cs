using Domain.Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class TokenService(IOptions<JwtOptions> _options, UserManager<ApplicationUser> _userManager) : ITokenService
    {
        public async Task<string> CreateTokenAsync(ApplicationUser user)
        {

            //1- get Options

            var JwtOptions = _options.Value;


            //2- get Claims

            var claims = new List<Claim>()
            {

                new Claim (ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())  // Add NameIdentifier claim her
            };


            var Roles = await _userManager.GetRolesAsync(user);


            foreach (var role in Roles)
            {

                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            //3- key


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));



            var SignCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var TokenDescriptor = new SecurityTokenDescriptor()
            {

                Subject = new ClaimsIdentity(claims),
                Issuer = JwtOptions.Issuer,
                Audience = JwtOptions.Audiance,
                Expires = DateTime.UtcNow.AddHours(JwtOptions.ExpirationInHours),

                SigningCredentials = SignCred,

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var Token = tokenHandler.CreateToken(TokenDescriptor);

            return tokenHandler.WriteToken(Token);
        }
    }
}
