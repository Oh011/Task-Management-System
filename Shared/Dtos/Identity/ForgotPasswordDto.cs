using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity
{
    public class ForgotPasswordDto
    {

        [Required]
        public string Email { get; set; }
    }
}
