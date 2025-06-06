namespace Shared.Dtos.Identity
{
    public class RegistrationResponseDto
    {
        public string FullName { get; set; }
        public string Message { get; set; } // Confirmation message
        public string Email { get; set; }

        public RegistrationResponseDto(string fullName, string message, string email)
        {
            FullName = fullName;
            Message = message;
            Email = email;
        }
    }
}
