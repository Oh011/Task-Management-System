using Shared.Dtos.Project;

namespace Services.Abstractions
{
    public interface IProjectDashboardService
    {


        Task<ProjectDashboardDto> GetDashboardDtoAsync(int projectId, string userId);



    }
}
