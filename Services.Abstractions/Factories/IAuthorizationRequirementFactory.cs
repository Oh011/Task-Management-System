using Domain.Entities.ProjectModels;
using Services.Abstractions.Common;

namespace Services.Abstractions.Factories
{
    public interface IAuthorizationRequirementFactory
    {
        ProjectActionRequirement CreateProjectRequirement(ProjectAction action, ProjectRole? role);
        ProjectTaskActionRequirement CreateTaskRequirement(ProjectTaskAction action, ProjectRole? role, int taskId, string userId);
    }


}
