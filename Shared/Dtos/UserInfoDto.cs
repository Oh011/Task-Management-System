namespace Shared.Dtos
{
    public class UserInfoDto
    {
        public string Id { get; set; }            // User Id (to send invitations, etc.)
        public string UserName { get; set; }      // Username or display name
        public string Email { get; set; }         // Email to show/contact
        public string ProfileImageUrl { get; set; } // Profile picture for UI
    }
}
