using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Invitations
{
    public class SendInvitationDto
    {
        public int ProjectId { get; set; }


        [Required]
        public string InvitedUserId { get; set; }

    }
}
