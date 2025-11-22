using Domain.Entities.IdentityModels;
using Domain.Entities.ProjectModels;

namespace Domain.Entities.TaskModels
{
    public class TaskItem : BaseEntity<int>
    {


        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; }

        public TasksStatus Status { get; set; } = TasksStatus.ToDo;

        public TaskType TaskType { get; set; } = TaskType.Personal;

        public DateTime? DueDate { get; set; }


        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        // Optionally, you can also assign a task to another user
        public string? AssignedToUserId { get; set; }
        public ApplicationUser? AssignedToUser { get; set; }


        public ProjectTask? ProjectTask { get; set; }
    }
}
