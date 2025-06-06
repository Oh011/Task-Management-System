using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity
{
    public class EmailConfirmDto
    {

        [Required]
        public string Email { get; set; }


        [Required]
        public string Token { get; set; }
    }
}
