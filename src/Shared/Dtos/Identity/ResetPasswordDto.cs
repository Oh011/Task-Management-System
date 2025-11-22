using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity
{
    public class ResetPasswordDto : EmailConfirmDto
    {

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]:;,.<>?/\\|`~-]).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]

        public string Password { get; set; }
    }
}
