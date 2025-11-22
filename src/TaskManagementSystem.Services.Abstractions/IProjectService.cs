using Shared.Dtos;
using Shared.Dtos.Project;
using Shared.ParameterTypes;

namespace Services.Abstractions
{
    public interface IProjectService
    {




        Task<ProjectSummaryDto> GetProjectSummaryAsync(int projectId);
        Task<ProjectSummaryDto> CreateProjectAsync(CreateProjectDto dto, string userId);


        //Task<ProjectSummaryDto> GetUserProjectsAsync(ProjectParameters parameters, string userId);

        Task<ProjectResultDto?> GetProjectByIdAsync(int projectId, string userId);


        Task<bool> ProjectExistsAsync(int projectId);


        Task<ProjectResultDto?> UpdateProjectAsync(int projectId, ProjectUpdateDto dto, string userId);


        Task<PaginatedResult<ProjectSummaryDto>> GetUserProjectsAsync(ProjectParameters parameters, string userId);


        Task<ProjectResultDto> UpdateProjectStatus(int projectId, UpdateProjectStatus dto, string userId);

        Task DeleteProjectAsync(int projectId, string userId);
    }
}
