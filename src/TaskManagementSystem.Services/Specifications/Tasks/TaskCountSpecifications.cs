using Domain.Contracts;
using Domain.Entities.TaskModels;
using System.Linq.Expressions;

namespace Services.Specifications.Tasks
{
    class TaskCountSpecifications : GroupSpecification<TaskItem, TasksStatus, TasksCount>
    {


        public TaskCountSpecifications(Expression<Func<TaskItem, bool>>? expression) : base(expression)
        {

            AddGroupBy(t => t.Status);


            AddGroupSelector(t => new TasksCount { Status = t.Key, count = t.Count() });


            SetResultOrderBy(t => t.count);

        }


    }



    public class TasksCount
    {


        public TasksStatus Status { get; set; }
        public int count { get; set; } = 0;
    }
}
