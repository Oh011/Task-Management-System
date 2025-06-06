using Domain.Entities.TaskModels;

namespace Services.Abstractions
{
    public interface ITaskAuthorizationService
    {
        void CheckViewPermission(TaskItem task, string? userId);
        void CheckModifyOrDeletePermission(TaskItem task, string? userId);
    }
}
