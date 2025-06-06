using Domain.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Utilities
{
    public class LinkBuilder : ILinkBuilder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;



        public LinkBuilder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string BuildEmailConfirmationLink(string token, string email)
        {
            return BuildLink("/api/auth/ConfirmEmail", token, email);
        }

        public string BuildPasswordResetLink(string token, string email)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            return encodedToken;
        }

        private string BuildLink(string path, string token, string email)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var scheme = request?.Scheme ?? "https";
            var host = request?.Host.Value ?? "localhost";

            var encodedToken = WebUtility.UrlEncode(token);


            return $"{scheme}://{host}{path}?token={encodedToken}&email={email}";
        }
    }

}
