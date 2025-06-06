using Domain.Entities.ProjectModels;
using Services.Abstractions.Common;
using Services.Abstractions.Factories;

namespace Services.Factories
{
    public class AuthorizationRequirementFactory : IAuthorizationRequirementFactory
    {
        public ProjectActionRequirement CreateProjectRequirement(ProjectAction action, ProjectRole? role)
        {
            return new ProjectActionRequirement(action, role);
        }

        public ProjectTaskActionRequirement CreateTaskRequirement(ProjectTaskAction action, ProjectRole? role, int taskId, string userId)
        {
            return new ProjectTaskActionRequirement(action, role, taskId, userId);
        }
    }
}
