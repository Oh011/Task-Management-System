using Shared.Dtos;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;

namespace Services.Abstractions
{
    public interface IProjectTasksService
    {


        Task<TaskResultDto> AssignTask(int projectId, string userId, AssignTaskDto dto);


        Task<ProjectTaskDetailsDto> GetProjectTaskByIdAsync(int taskId, int projectId, string userId);
        Task<ProjectTaskResultDto> AddTask(CreateProjectTaskDto dto, int projectId, string userId);

        Task<TaskResultDto?> UpdateTask(UpdateTaskDto dto, int taskId, int projectId, string userId);


        Task<PaginatedResult<ProjectTaskResultDto>> GetProjectTasksAsync(int projectId, string userId, TaskParameters parameters);

        Task<PaginatedResult<TaskResultDto>> GetAssignedTasksForUserAsync(int projectId, string userId, TaskParameters parameters);
        Task<TaskResultDto?> ChangeTaskStatus(int taskId, string userId, int projectId, UpdateTaskStatus Dto);
        Task DeleteTask(int taskId, int ProjectId, string userId);
    }
}
