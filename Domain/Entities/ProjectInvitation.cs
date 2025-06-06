using Domain.Entities.IdentityModels;
using Domain.Entities.ProjectModels;
using Domain.Entities.TaskModels;

namespace Domain.Entities
{
    public class ProjectInvitation : BaseEntity<int>

    {




        public int ProjectId { get; set; }
        public string InvitedUserId { get; set; }


        public DateTime? ExpiresAt { get; set; }

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        public Project Project { get; set; }
        public ApplicationUser InvitedUser { get; set; }
    }
}
