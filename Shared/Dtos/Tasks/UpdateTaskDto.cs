using Domain.Entities.TaskModels;
using Shared.CustomValidationsAttributes;

namespace Shared.Dtos.Tasks
{
    public class UpdateTaskDto
    {


        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskPriority? Priority { get; set; }

        [FutureDate(ErrorMessage = "Due date must be in the future.")]
        public DateTime? DueDate { get; set; }
    }
}
