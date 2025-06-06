using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Identity
{


    public class LogOutRequest
    {

        [Required]
        public string DeviceId { get; set; }
    }

}
