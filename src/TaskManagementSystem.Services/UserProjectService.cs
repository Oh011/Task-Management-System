using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.ProjectModels;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Abstractions.Common;
using Services.Specifications;
using Services.Specifications.User;
using Shared.Dtos;
using Shared.Dtos.Project;
using Shared.ParameterTypes;

namespace Services
{
    public class UserProjectService(IUnitOfWork unitOfWork, IUserRepository userRepository, IProjectAuthorizationService projectAuthorizationService) : IUserProjectService
    {
        public async Task<ProjectUserDto?> AssignRoleToUserAsync(string userId, int projectId, AssignRoleDto dto)
        {


            if (userId == dto.TargetUserId)
                throw new BadRequestException("You cannot assign a role to yourself.");

            await projectAuthorizationService.AuthorizeProjectAction(userId, projectId, ProjectAction.AssignProjectRoles);

            var targetUser = await GetProjectUserEntityAsync(projectId, dto.TargetUserId);

            if (targetUser == null)
                throw new NotFoundException("Target user is not a member of this project.");


            if (dto.NewRole == ProjectRole.Owner)
            {
                throw new BadRequestException("You cannot assign the Owner role to another user.");
            }


            targetUser.Role = dto.NewRole;

            var repo = unitOfWork.GetRepository<ProjectUser, int>();

            repo.Update(targetUser);

            await unitOfWork.SaveChanges();



            return await GetProjectUserAsync(projectId, dto.TargetUserId);

        }

        public async Task<ProjectUserDto?> CreateProjectUserAsync(string userId, int projectId, ProjectRole role)
        {

            if (await ProjectExistsAsync(projectId) == false)
                throw new ProjectNotFoundException(projectId);


            var existingProjectUser = await IsUserInProjectAsync(projectId, userId);

            if (existingProjectUser == true)
                throw new ResourceExists("User already in project.");


            var projectUser = new ProjectUser
            {
                UserId = userId,
                ProjectId = projectId,
                Role = role
            };


            await unitOfWork.GetRepository<ProjectUser, int>().AddAsync(projectUser);
            await unitOfWork.SaveChanges();

            return await GetProjectUserAsync(projectId, userId);


        }

        public async Task<PaginatedResult<ProjectUserDto>> GetProjectUsersAsync(int projectId, string userId, ProjectUserParameters parameters)
        {


            var Repo = unitOfWork.GetRepository<ProjectUser, int>();


            if (await ProjectExistsAsync(projectId) == false)
                throw new ProjectNotFoundException(projectId);


            if (await IsUserInProjectAsync(projectId, userId) == false)
                throw new ForbiddenException("You are not authorized to get users.");

            var specification = new ProjectUsersSpecification(projectId, parameters);
            var result = await Repo.GetAllWithProjectionSpecifications<ProjectUserDto>(specification);



            var count = (await Repo.CountAsync(specification));



            return new PaginatedResult<ProjectUserDto>(

                parameters.PageIndex,
                parameters.PageSize,
                count,
                result

                );

        }



        public async Task<PaginatedResult<UserInfoDto>> GetUsersNotInProjectAsync(string userId, int projectId, UserSearchParameters parameters)
        {


            await projectAuthorizationService.AuthorizeProjectAction(userId, projectId, ProjectAction.ManageMembers);

            var specification = new UserSpecifications(projectId, parameters);
            var users = await userRepository.GetAllWithProjectionSpecifications(specification);

            specification.isPaginated = false;

            var count = (await userRepository.GetAllWithProjectionSpecifications(specification))
                .Count();




            return new PaginatedResult<UserInfoDto>(

                parameters.PageIndex,
                parameters.PageSize,
                count,
                users

                );
        }



        public async Task<ProjectUser?> GetProjectUserEntityAsync(int projectId, string userId)
        {
            var repo = unitOfWork.GetRepository<ProjectUser, int>();

            var user = await repo.FindAsync(pu => pu.UserId == userId && pu.ProjectId == projectId);

            return user;
        }

        public async Task<ProjectUserDto?> GetProjectUserAsync(int projectId, string userId)
        {

            var Repo = unitOfWork.GetRepository<ProjectUser, int>();

            var result = await Repo.GetWithIdProjectionSpecifications(new ProjectUsersSpecification(projectId, userId));
            return result;

        }

        public async Task RemoveProjectUserAsync(string userId, string targetUserId, int projectId)
        {

            if (userId == targetUserId)
                throw new BadRequestException("You cannot remove yourself from the project.");

            await projectAuthorizationService.AuthorizeProjectAction(userId, projectId, ProjectAction.RemoveProjectMember);

            var targetUser = await GetProjectUserEntityAsync(projectId, targetUserId);

            if (targetUser == null)
                throw new NotFoundException("Target user is not a member of this project.");



            var repo = unitOfWork.GetRepository<ProjectUser, int>();

            repo.Delete(targetUser);

            await unitOfWork.SaveChanges();


        }


        private async Task<bool> ProjectExistsAsync(int projectId)
        {
            var repo = unitOfWork.GetRepository<Project, int>();
            return await repo.ExistsAsync(p => p.Id == projectId);
        }


        public async Task<bool> IsUserInProjectAsync(int projectId, string userId)
        {


            return await unitOfWork.GetRepository<ProjectUser, int>().ExistsAsync(U => U.ProjectId == projectId && U.UserId == userId);
        }


        public async Task<Dictionary<string, int>> CountUsersByRoleAsync(int projectId)
        {
            var users = await unitOfWork.GetRepository<ProjectUser, int>()
                .GetAllWithGrouping(new ProjectUsersCountSpecifications(projectId));

            var allUserRoles = Enum.GetValues(typeof(ProjectRole)).Cast<ProjectRole>();

            var usersGroupedDict = users.ToDictionary(u => u.Role, u => u.Count);

            return allUserRoles.ToDictionary(
                role => role.ToString(),
                role => usersGroupedDict.TryGetValue(role, out var count) ? count : 0
            );
        }

    }
}
