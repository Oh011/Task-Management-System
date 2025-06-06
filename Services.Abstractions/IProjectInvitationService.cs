using Shared.Dtos;
using Shared.Dtos.Invitations;
using Shared.ParameterTypes;

namespace Services.Abstractions
{
    public interface IProjectInvitationService
    {
        Task SendInvitationAsync(SendInvitationDto dto, string currentUserId);


        Task<UserProjectInvitationsDto?> GetInvitationsById(int id);
        Task<IEnumerable<UserProjectInvitationsDto>> GetUserProjectInvitations(string userId);


        Task AcceptInvitation(int invitationId, string userId);




        Task RejectInvitationInvitation(int invitationId, string userId);


        Task DeleteProjectInvitationAsync(int InvitationId, string UserId);

        Task<PaginatedResult<ProjectInvitationsDto>> GetProjectInvitations(int projectId, string userId, ProjectInvitationParameters parameters);


    }
}
