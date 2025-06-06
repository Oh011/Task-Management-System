using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProjectModels;
using Domain.Entities.TaskModels;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Abstractions.Common;
using Shared.Dtos;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;

namespace Services
{
    class ProjectTasksService(IUnitOfWork unitOfWork, IUserService userService, IUserProjectService userProjectService, IProjectService projectService, ITaskService taskService, IProjectAuthorizationService projectAuthorizationService, IMapper mapper) : IProjectTasksService
    {


        public async Task<ProjectTaskResultDto> AddTask(CreateProjectTaskDto dto, int projectId, string userId)
        {



            await projectService.ProjectExistsAsync(projectId);


            await projectAuthorizationService.AuthorizeProjectAction(userId, projectId, ProjectAction.AddTask);
            var CreatedTask = await taskService.CreateTask(dto, userId, TaskType.Project);



            var projectTask = new ProjectTask
            {
                TaskItemId = CreatedTask.Id,
                ProjectId = projectId,

            };

            await unitOfWork.GetRepository<ProjectTask, int>().AddAsync(projectTask);
            await unitOfWork.SaveChanges();

            var createdProjectTask = mapper.Map<ProjectTaskResultDto>(CreatedTask);


            return createdProjectTask;




        }

        public async Task<TaskResultDto?> ChangeTaskStatus(int taskId, string userId, int projectId, UpdateTaskStatus Dto)
        {


            await AuthorizeProjectTask(userId, projectId, taskId, ProjectTaskAction.ChangeTaskStatus);
            var result = await taskService.ChangeTaskStatus(taskId, userId, Dto, true);

            return result;

        }

        public async Task DeleteTask(int taskId, int ProjectId, string userId)
        {

            await AuthorizeProjectTask(userId, ProjectId, taskId, ProjectTaskAction.DeleteTask);
            await taskService.DeleteTask(taskId, userId, true);

        }

        public async Task<PaginatedResult<ProjectTaskResultDto>> GetProjectTasksAsync(int projectId, string userId, TaskParameters parameters)
        {


            if (await userProjectService.IsUserInProjectAsync(projectId, userId) == false)
                throw new ForbiddenException($"You are not authorized to view this project tasks.");



            return await taskService.GetProjectTasksAsync(projectId, userId, parameters);

        }

        public async Task<PaginatedResult<TaskResultDto>> GetAssignedTasksForUserAsync(int projectId, string userId, TaskParameters parameters)
        {


            if (await userProjectService.IsUserInProjectAsync(projectId, userId) == false)
                throw new ForbiddenException($"You are not authorized to view this project tasks.");


            return await taskService.GetAssignedProjectTasksAsync(projectId, userId, parameters);

        }

        public async Task<TaskResultDto?> UpdateTask(UpdateTaskDto dto, int taskId, int projectId, string userId)
        {


            await AuthorizeProjectTask(userId, projectId, taskId, ProjectTaskAction.UpdateTask);
            var result = await taskService.UpdateTask(taskId, dto, userId, true);

            return result;

        }





        private async Task AuthorizeProjectTask(string userId, int projectId, int taskId, ProjectTaskAction action)
        {

            await projectService.ProjectExistsAsync(projectId);

            await EnsureTaskBelongsToProject(taskId, projectId);
            await projectAuthorizationService.AuthorizeProjectTaskAction(userId, projectId, taskId, action);
        }

        private async Task EnsureTaskBelongsToProject(int taskId, int projectId)
        {
            var projectTaskRepo = unitOfWork.GetRepository<ProjectTask, int>();
            var exists = await projectTaskRepo.ExistsAsync(PT => PT.TaskItemId == taskId && PT.ProjectId == projectId);
            if (exists == false)
                throw new ForbiddenException($"Task {taskId} does not belong to project {projectId}.");
        }

        public async Task<TaskResultDto> AssignTask(int projectId, string userId, AssignTaskDto dto)
        {


            await AuthorizeProjectTask(userId, projectId, dto.TaskId, ProjectTaskAction.AssignTask);


            var targetUser = await userProjectService.IsUserInProjectAsync(projectId, userId);

            if (targetUser == false)
                throw new NotFoundException("Target user is not a member of this project.");


            return await taskService.AssignTaskAsync(dto);

        }

        public async Task<ProjectTaskDetailsDto> GetProjectTaskByIdAsync(int taskId, int projectId, string userId)
        {
            var userExists = await userProjectService.IsUserInProjectAsync(projectId, userId);

            if (userExists == false)
                throw new NotFoundException("Target user is not a member of this project.");

            await EnsureTaskBelongsToProject(taskId, projectId);

            var task = await taskService.GetProjectTaskByIdAsync(taskId, projectId, userId);



            return task;




        }
    }
}
