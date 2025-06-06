using Domain.Contracts;
using Domain.Entities;
using Shared.Dtos.Invitations;
using Shared.ParameterTypes;

namespace Services.Specifications.ProjectSpecifications
{
    class ProjectInvitationSpecifications : ProjectionSpecifications<ProjectInvitation, ProjectInvitationsDto>
    {
        public ProjectInvitationSpecifications(int ProjectId, ProjectInvitationParameters parameters) :

            base(I => I.ProjectId == ProjectId && (!parameters.Status.HasValue || parameters.Status == I.Status) && (I.ExpiresAt == null || I.ExpiresAt > DateTime.UtcNow))
        {


            AddInclude(I => I.InvitedUser);


            AddProjection(I => new ProjectInvitationsDto
            {

                InvitationId = I.Id,
                InvitedUserId = I.InvitedUserId,
                InvitedUserEmail = I.InvitedUser.Email,
                SentAt = I.CreatedAt,
                ExpiresAt = I.ExpiresAt ?? DateTime.MaxValue,
                Status = I.Status.ToString()
            });


            if (parameters.SortBySentAtAsc)
                SetOrderBy(inv => inv.CreatedAt);
            else
                SetOrderByDescending(inv => inv.CreatedAt);


            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }


    }
}
