using Domain.Entities.TaskModels;
using System.ComponentModel;
using TasksStatus = Domain.Entities.TaskModels.TasksStatus;
namespace Shared.ParameterTypes
{
    public class TaskParameters : PaginationQueryParameters
    {


        public string? Search { get; set; }

        public TaskPriority? Priority { get; set; } = null;

        public TasksStatus? Status { get; set; }

        public TaskSortOptions? SortOptions { get; set; }





    }


    public enum TaskSortOptions
    {


        [Description("Sort by DueDate Ascending")]
        DueDateAsc,

        [Description("Sort by DueDate Descending")]
        DueDateDesc,


    }
}
