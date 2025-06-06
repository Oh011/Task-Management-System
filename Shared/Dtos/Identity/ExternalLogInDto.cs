using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity
{
    public class ExternalLogInDto
    {

        [Required]
        public string Name { get; set; }


        [Required]
        public string Email { get; set; }



        [Required]
        public string DeviceId { get; set; }  // Added DeviceId

    }



}
