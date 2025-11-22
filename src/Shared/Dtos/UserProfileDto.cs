namespace Shared.Dtos
{
    public class UserProfileDto
    {

        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? PhoneNumber { get; set; }

        public int CreatedProjectCount { get; set; }
        public int AssignedTaskCount { get; set; }
    }
}
