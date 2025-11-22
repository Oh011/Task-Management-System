using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity
{
    public class RefreshTokenRequest
    {

        [Required]
        public string DeviceId { get; set; }  // Device ID
    }
}
