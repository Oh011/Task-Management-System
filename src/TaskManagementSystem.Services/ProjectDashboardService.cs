using Domain.Exceptions;
using Services.Abstractions;
using Shared.Dtos.Project;

namespace Services
{
    internal class ProjectDashboardService(ITaskService taskService, IProjectService projectService, IUserProjectService userProjectService) : IProjectDashboardService
    {

        public async Task<ProjectDashboardDto> GetDashboardDtoAsync(int projectId, string userId)
        {


            var project = await projectService.GetProjectSummaryAsync(projectId);


            if (project == null)
                throw new ProjectNotFoundException(projectId);


            var isUserInProject = await userProjectService.IsUserInProjectAsync(projectId, userId);


            if (!isUserInProject)
                throw new ProjectUnauthorizedActionException();


            var taskDict = await taskService.CountTasksByStatusAsync(t => t.ProjectTask.ProjectId == projectId);



            var tasksTotalCount = taskDict.Values.Sum();



            var userDict = await userProjectService.CountUsersByRoleAsync(projectId);



            var usersTotalCount = userDict.Values.Sum();


            double GetPercentage(string status) =>
        tasksTotalCount == 0 ? 0 : Math.Round((taskDict.GetValueOrDefault(status, 0) * 100.0) / tasksTotalCount, 2);


            var dashBoard = new ProjectDashboardDto
            {
                ProjectName = project.Name,
                ProjectId = projectId,
                StartDate = project.StartDate,
                Status = project.Status,

                TaskCountsByStatus = taskDict,

                MemberCountsByRole = userDict,

                TotalMembers = usersTotalCount,

                TotalTasks = tasksTotalCount,


                DonePercentage = GetPercentage("Done"),
                InProgressPercentage = GetPercentage("InProgress"),
                ToDoPercentage = GetPercentage("ToDo"),
                BlockedPercentage = GetPercentage("Blocked"),



            };


            return dashBoard;


        }
    }
}


