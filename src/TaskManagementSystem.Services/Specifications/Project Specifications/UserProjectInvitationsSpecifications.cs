using Domain.Contracts;
using Domain.Entities;
using Shared.Dtos.Invitations;

namespace Services.Specifications.ProjectSpecifications
{
    public class UserProjectInvitationsSpecifications : ProjectionSpecifications<ProjectInvitation, UserProjectInvitationsDto>
    {
        public UserProjectInvitationsSpecifications(int id) : base(I => I.Id == id && I.Status == InvitationStatus.Pending && (I.ExpiresAt == null || I.ExpiresAt > DateTime.UtcNow))
        {


            AddInclude(I => I.Project);

            AddProjection(I => new UserProjectInvitationsDto
            {
                InvitationId = I.Id,
                ProjectId = I.ProjectId,
                ProjectName = I.Project.Name,
                status = I.Status.ToString(),
            });



        }
        public UserProjectInvitationsSpecifications(string userId) : base(I => I.InvitedUserId == userId && I.Status == InvitationStatus.Pending && (I.ExpiresAt == null || I.ExpiresAt > DateTime.UtcNow))
        {


            AddInclude(I => I.Project);

            AddProjection(I => new UserProjectInvitationsDto
            {
                InvitationId = I.Id,
                ProjectId = I.ProjectId,
                ProjectName = I.Project.Name,
                status = I.Status.ToString(),
            });

            SetOrderByDescending(I => I.ExpiresAt ?? DateTime.MaxValue);

        }
    }
}
