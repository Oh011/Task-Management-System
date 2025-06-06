using System.ComponentModel.DataAnnotations;
using TasksStatus = Domain.Entities.TaskModels.TasksStatus;
namespace Shared.Dtos.Tasks
{
    public class UpdateTaskStatus
    {

        [Required]
        public TasksStatus Status { get; set; }
    }
}
