using Domain.Entities.ProjectModels;
using Services.Abstractions.Common;


namespace Services.Abstractions
{
    public interface IProjectAuthorizationService
    {


        Task<ProjectRole?> GetUserRoleAsync(int projectId, string userId);
        Task AuthorizeProjectAction(string userId, int projectId, ProjectAction action);



        Dictionary<string, Dictionary<string, bool>> GetAllowedActions(ProjectRole role);


        Task AuthorizeProjectTaskAction(string userId, int projectId, int taskId, ProjectTaskAction action);

    }
}
