using Domain.Entities;
using Domain.Entities.ProjectModels;
using Shared.Dtos;
using Shared.Dtos.Project;
using Shared.ParameterTypes;

namespace Services.Abstractions
{
    public interface IUserProjectService
    {
        Task<ProjectUserDto?> CreateProjectUserAsync(string userId, int projectId, ProjectRole role);

        Task<bool> IsUserInProjectAsync(int projectId, string userId);

        public Task<ProjectUser?> GetProjectUserEntityAsync(int projectId, string userId);

        Task<ProjectUserDto?> AssignRoleToUserAsync(string userId, int projectId, AssignRoleDto dto);

        //internal Task<ProjectUser?> GetProjectUserEntityAsync(int projectId, string userId);
        Task RemoveProjectUserAsync(string userId, string targetUserId, int projectId);
        Task<ProjectUserDto?> GetProjectUserAsync(int projectId, string userId);

        //Task<List<ProjectUserDto>> GetProjectUsersAsync(int projectId);

        Task<PaginatedResult<UserInfoDto>> GetUsersNotInProjectAsync(string userId, int projectId, UserSearchParameters parameters);
        public Task<PaginatedResult<ProjectUserDto>> GetProjectUsersAsync(int projectId, string userId, ProjectUserParameters parameters);


        Task<Dictionary<string, int>> CountUsersByRoleAsync(int projectId);
    }

}
