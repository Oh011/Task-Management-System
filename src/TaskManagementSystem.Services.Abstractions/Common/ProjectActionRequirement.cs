using Domain.Entities.ProjectModels;

namespace Services.Abstractions.Common
{
    public abstract class AuthorizationRequirement { }

    public class ProjectActionRequirement : AuthorizationRequirement
    {
        public ProjectAction Action { get; }
        public ProjectRole? Role { get; }

        public ProjectActionRequirement(ProjectAction action, ProjectRole? role)
        {
            Action = action;
            Role = role;
        }
    }

    public class ProjectTaskActionRequirement : AuthorizationRequirement
    {
        public ProjectTaskAction Action { get; }
        public ProjectRole? Role { get; }
        public int TaskId { get; }
        public string UserId { get; }

        public ProjectTaskActionRequirement(ProjectTaskAction action, ProjectRole? role, int taskId, string userId)
        {
            Action = action;
            Role = role;
            TaskId = taskId;
            UserId = userId;
        }
    }






}
