using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.ProjectModels;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Abstractions.Common;
using Services.Abstractions.Factories;


namespace Services
{
    internal class ProjectAuthorizationService : IProjectAuthorizationService
    {



        private readonly ITaskService _taskService;

        private readonly IUnitOfWork unitOfWork;


        private readonly IAuthorizationRequirementFactory _authorizationRequirementFactory;


        public ProjectAuthorizationService(ITaskService taskService, IUnitOfWork unitOfWork, IAuthorizationRequirementFactory authorizationRequirementFactory)
        {

            this.unitOfWork = unitOfWork;
            this._taskService = taskService;
            _authorizationRequirementFactory = authorizationRequirementFactory;
        }

        public async Task AuthorizeProjectAction(string userId, int projectId, ProjectAction action)
        {

            var userRole = await this.GetUserRoleAsync(projectId, userId);

            if (userRole == null)
            {
                // User is not part of the project, so no authorization is granted.
                throw new ProjectUnauthorizedActionException();
            }

            var requirement = _authorizationRequirementFactory.CreateProjectRequirement(action, userRole);

            var authorized = this.CheckProjectAction(requirement);

            if (!authorized)
                throw new ProjectUnauthorizedActionException();
        }


        public async Task AuthorizeProjectTaskAction(string userId, int projectId, int taskId, ProjectTaskAction action)
        {

            var userRole = await this.GetUserRoleAsync(projectId, userId);

            if (userRole == null)
            {
                // User is not part of the project, so no authorization is granted.
                throw new ProjectUnauthorizedActionException();
            }

            var requirement = _authorizationRequirementFactory.CreateTaskRequirement(action, userRole, taskId, userId);

            var authorized = await this.CheckTaskAction(requirement);

            if (!authorized)
                throw new ProjectUnauthorizedActionException();
        }




        private bool CheckProjectAction(ProjectActionRequirement requirement)
        {

            var userRole = requirement.Role;



            if (ProjectRolePermissions.TryGetValue(userRole.Value, out var allowedActions))
            {
                return allowedActions.Contains(requirement.Action);
            }


            return false;
        }



        private static readonly Dictionary<ProjectRole, HashSet<ProjectAction>> ProjectRolePermissions = new()
        {

        { ProjectRole.Owner, new HashSet<ProjectAction> {
        ProjectAction.UpdateProject, ProjectAction.DeleteProject, ProjectAction.ChangeProjectStatus,
        ProjectAction.ManageMembers, ProjectAction.AssignProjectRoles, ProjectAction.RemoveProjectMember, ProjectAction.AddTask
    } },

        { ProjectRole.Admin, new HashSet<ProjectAction> {
            ProjectAction.ChangeProjectStatus, ProjectAction.ManageMembers, ProjectAction.AddTask
        } },

        { ProjectRole.Member, new HashSet<ProjectAction>() } // no project-level permissions


        };


        public Dictionary<string, Dictionary<string, bool>> GetAllowedActions(ProjectRole role)
        {
            var result = new Dictionary<string, Dictionary<string, bool>>
            {
                ["Project"] = new Dictionary<string, bool>(),
                ["Task"] = new Dictionary<string, bool>()
            };

            foreach (ProjectAction action in Enum.GetValues(typeof(ProjectAction)))
            {
                result["Project"][action.ToString()] = ProjectRolePermissions[role].Contains(action);
            }

            foreach (ProjectTaskAction action in Enum.GetValues(typeof(ProjectTaskAction)))
            {
                if (action == ProjectTaskAction.ChangeTaskStatus)
                    continue;

                result["Task"][action.ToString()] = ProjectRoleTaskPermissions[role].Contains(action);
            }

            return result;
        }







        private static readonly Dictionary<ProjectRole, HashSet<ProjectTaskAction>> ProjectRoleTaskPermissions = new()
{
    { ProjectRole.Owner, new HashSet<ProjectTaskAction> {
        ProjectTaskAction.UpdateTask, ProjectTaskAction.DeleteTask, ProjectTaskAction.ChangeTaskStatus, ProjectTaskAction.AssignTask
    } },
    { ProjectRole.Admin, new HashSet<ProjectTaskAction> {
        ProjectTaskAction.UpdateTask, ProjectTaskAction.DeleteTask, ProjectTaskAction.ChangeTaskStatus, ProjectTaskAction.AssignTask
    } },
    { ProjectRole.Member, new HashSet<ProjectTaskAction> {
        ProjectTaskAction.ChangeTaskStatus
    } } // members can only change status of assigned tasks
};


        private async Task<bool> CheckTaskAction(ProjectTaskActionRequirement requirement)
        {

            var userRole = requirement.Role;



            if (requirement.Action == ProjectTaskAction.ChangeTaskStatus)
            {

                if (userRole == ProjectRole.Owner || userRole == ProjectRole.Admin || await _taskService.IsTaskAssignedToAsync(requirement.TaskId, requirement.UserId))
                {
                    return true;
                }

                return false;


            }


            if (ProjectRoleTaskPermissions.TryGetValue(userRole.Value, out var allowedActions))
            {
                return allowedActions.Contains(requirement.Action);
            }


            return false;




        }

        public async Task<ProjectRole?> GetUserRoleAsync(int projectId, string userId)
        {
            var ProjectUser = await unitOfWork.GetRepository<ProjectUser, int>()
             .FindAsync(U => U.UserId == userId && U.ProjectId == projectId);

            return ProjectUser?.Role;
        }


    }
}
