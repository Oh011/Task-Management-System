using Domain.Entities.TaskModels;
using Shared.CustomValidationsAttributes;
using System.ComponentModel.DataAnnotations;
namespace Shared.Dtos.Tasks
{
    public class CreateTaskDto
    {

        [Required]
        public string Title { get; set; } = string.Empty;



        public string? Description { get; set; }



        public TaskPriority Priority { get; set; }


        [FutureDate(ErrorMessage = "Due date must be in the future.")]
        public DateTime? DueDate { get; set; }





    }
}


//ASP.NET Core can bind both strings and integers to enums automatically.


//Integer binding works when the integer corresponds to the enum's numeric value (e.g., 0, 1, 2)