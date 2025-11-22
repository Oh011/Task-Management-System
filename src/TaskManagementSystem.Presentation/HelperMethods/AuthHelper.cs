using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Presentation.HelperMethods
{


    public static class AuthHelpers
    {
        public static string? GetUserId(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return userId;
        }

    }
}
