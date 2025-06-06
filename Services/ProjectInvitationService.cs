using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.ProjectModels;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Abstractions.Common;
using Services.Specifications.ProjectSpecifications;
using Shared.Dtos;
using Shared.Dtos.Invitations;
using Shared.ParameterTypes;

namespace Services
{
    class ProjectInvitationService(IUserProjectService userProjectService,
    IProjectAuthorizationService projectAuthorizationService,
    IUnitOfWork unitOfWork) : IProjectInvitationService
    {
        private async Task<ProjectInvitation> UpdateInvitationStatus(int invitationId, string userId, InvitationStatus status)
        {

            var repo = unitOfWork.GetRepository<ProjectInvitation, int>();


            var Invitation = await ValidateInvitation(invitationId, userId);


            Invitation.Status = status;



            repo.Update(Invitation);
            await unitOfWork.SaveChanges();


            return Invitation;



        }



        public async Task AcceptInvitation(int invitationId, string userId)
        {

            var invitation = await UpdateInvitationStatus(invitationId, userId, InvitationStatus.Accepted);

            await userProjectService.CreateProjectUserAsync(userId, invitation.ProjectId, ProjectRole.Member);

        }


        public async Task RejectInvitationInvitation(int invitationId, string userId)
        {

            await UpdateInvitationStatus(invitationId, userId, InvitationStatus.Rejected);

        }



        public async Task<UserProjectInvitationsDto?> GetInvitationsById(int id)
        {

            var userInvitations = await unitOfWork.GetRepository<ProjectInvitation, int>()
             .GetWithIdProjectionSpecifications(new UserProjectInvitationsSpecifications(id));


            if (userInvitations == null)
                throw new NotFoundException($"Invitation with Id :{id} not found");

            return userInvitations;

        }

        public async Task<IEnumerable<UserProjectInvitationsDto>> GetUserProjectInvitations(string userId)
        {

            var userInvitations = await unitOfWork.GetRepository<ProjectInvitation, int>()
                .GetAllWithProjectionSpecifications(new UserProjectInvitationsSpecifications(userId));

            return userInvitations;

        }



        public async Task SendInvitationAsync(SendInvitationDto dto, string currentUserId)
        {

            //Add users


            await projectAuthorizationService.AuthorizeProjectAction(currentUserId, dto.ProjectId, ProjectAction.ManageMembers);


            if (await userProjectService.IsUserInProjectAsync(dto.ProjectId, dto.InvitedUserId) == true)
                throw new ResourceExists("This user is already a member of the project.");


            var repo = unitOfWork.GetRepository<ProjectInvitation, int>();

            var existing = await GetExistingInvitationAsync(dto.ProjectId, dto.InvitedUserId);


            if (existing != null && existing.ExpiresAt > DateTime.UtcNow)
            {
                repo.Delete(existing);
                // optional immediate save: await unitOfWork.SaveChanges();
            }


            var invitation = new ProjectInvitation
            {
                ProjectId = dto.ProjectId,
                InvitedUserId = dto.InvitedUserId,
                ExpiresAt = DateTime.UtcNow.AddDays(10),
            };


            await repo.AddAsync(invitation);
            await unitOfWork.SaveChanges();
        }


        private async Task<ProjectInvitation?> GetExistingInvitationAsync(int projectId, string invitedUserId)
        {
            var repo = unitOfWork.GetRepository<ProjectInvitation, int>();
            return await repo.FindAsync(i => i.ProjectId == projectId && i.InvitedUserId == invitedUserId);
        }





        private async Task<ProjectInvitation> ValidateInvitation(int invitationId, string userId)
        {

            var repo = unitOfWork.GetRepository<ProjectInvitation, int>();


            var Invitation = await repo
                .FindAsync(I => I.InvitedUserId == userId && I.Id == invitationId);

            if (Invitation == null)
                throw new NotFoundException($"No invitation with Id :{invitationId} for {userId} userId");


            if (Invitation.Status != InvitationStatus.Pending || Invitation.ExpiresAt < DateTime.UtcNow)
                throw new InvalidInvitationOperationException("This invitation is either not pending or has expired.");


            return Invitation;

        }



        public async Task<PaginatedResult<ProjectInvitationsDto>> GetProjectInvitations(int projectId, string userId, ProjectInvitationParameters parameters)
        {



            await projectAuthorizationService.AuthorizeProjectAction(userId, projectId, ProjectAction.ManageMembers);


            var repo = unitOfWork.GetRepository<ProjectInvitation, int>();

            var specifications = new ProjectInvitationSpecifications(projectId, parameters);

            var totalCount = await repo.CountAsync(specifications);

            var Invitations = await repo.GetAllWithProjectionSpecifications(specifications);



            return new PaginatedResult<ProjectInvitationsDto>(parameters.PageIndex, parameters.PageSize

                , totalCount, Invitations);


        }

        public async Task DeleteProjectInvitationAsync(int projectId, string UserId)
        {


            var repo = unitOfWork.GetRepository<ProjectInvitation, int>();

            var result = await repo.FindAsync(I => I.InvitedUserId == UserId && I.ProjectId == projectId);


            if (result == null)
                return;

            repo.Delete(result);

            await unitOfWork.SaveChanges();


        }
    }
}
