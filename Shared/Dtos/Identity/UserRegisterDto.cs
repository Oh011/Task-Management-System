using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity
{
    public class UserRegisterDto
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]:;,.<>?/\\|`~-]).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]

        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and digits.")]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
