namespace Shared.Dtos.Identity
{
    public record UserLogInDto
    {

        public string Email { get; set; }

        public string Password { get; set; }

        public string DeviceId { get; set; }  // New field to track the device
        public string DeviceInfo { get; set; }  // New field for user agent (device/browser details)
    }
}
