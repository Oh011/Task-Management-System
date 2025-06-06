using Domain.Entities.ProjectModels;
using Shared.Dtos.Tasks;
using TaskStatus = Domain.Entities.TaskModels.TasksStatus;
namespace Services.Abstractions
{
    public interface IStatusTransitionService
    {

        IReadOnlyDictionary<TaskStatus, HashSet<TaskStatus>> GetTaskTransitions();

        IReadOnlyDictionary<ProjectStatus, HashSet<ProjectStatus>> GetProjectTransitions();

        void AddAllowedTaskTransitions(IEnumerable<TaskDetailsDto> tasks);


        void CheckAllowedTaskTransitions(TaskStatus oldStatus, TaskStatus newStatus);
    }
}
