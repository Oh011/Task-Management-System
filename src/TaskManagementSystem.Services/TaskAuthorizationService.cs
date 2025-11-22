using Domain.Entities.TaskModels;
using Domain.Exceptions;
using Services.Abstractions;

namespace Services
{
    class TaskAuthorizationService : ITaskAuthorizationService
    {



        public void CheckModifyOrDeletePermission(TaskItem task, string? userId)
        {
            if (task.TaskType == TaskType.Personal && task.CreatedByUserId != userId)
                throw new ForbiddenException("You are not allowed to modify this task");


        }

        public void CheckViewPermission(TaskItem task, string? userId)
        {
            if (task.TaskType == TaskType.Personal && task.CreatedByUserId != userId)
                throw new ForbiddenException("You are not allowed to view this personal task");




        }
    }
}
