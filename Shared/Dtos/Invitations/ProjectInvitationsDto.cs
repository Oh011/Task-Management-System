namespace Shared.Dtos.Invitations
{
    public class ProjectInvitationsDto
    {

        public int InvitationId { get; set; }
        public string InvitedUserId { get; set; }
        public string InvitedUserEmail { get; set; } // Optional: helpful for admins
        public DateTime SentAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Status { get; set; } // Use string here for readability
    }
}
