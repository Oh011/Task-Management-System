using Domain.Entities.TaskModels;
using Shared.Dtos;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;
using System.Linq.Expressions;

namespace Services.Abstractions
{
    public interface ITaskService
    {





        Task<TaskResultDto> CreateTask(CreateTaskDto task, string userId, TaskType taskType = TaskType.Personal);


        Task<PaginatedResult<TaskResultDto>> GetAssignedProjectTasksAsync(int projectId, string userId, TaskParameters parameters);

        Task<TaskDetailsDto?> GetTaskById(int taskId, string userId, bool bypassAuthorizationChecks = false);

        Task<PaginatedResult<TaskResultDto>> GetUserTasksAsync(TaskParameters parameters, string userId);

        Task<ProjectTaskDetailsDto> GetProjectTaskByIdAsync(int taskId, int projectId, string userId);
        Task<TaskResultDto> AssignTaskAsync(AssignTaskDto dto);


        Task<PaginatedResult<ProjectTaskResultDto>> GetProjectTasksAsync(int projectId, string? userId, TaskParameters parameters);

        Task<TaskResultDto?> UpdateTask(int id, UpdateTaskDto task, string? userId, bool bypassAuthorizationChecks = false);

        Task DeleteTask(int id, string? userId, bool bypassAuthorizationChecks = false);



        Task<Dictionary<string, int>> CountTasksByStatusAsync(Expression<Func<TaskItem, bool>>? filter);
        Task<bool> IsTaskAssignedToAsync(int taskId, string? userId);


        Task<TaskResultDto?> ChangeTaskStatus(int taskId, string? userId, UpdateTaskStatus Dto, bool bypassAuthorizationChecks = false);
    }
}
