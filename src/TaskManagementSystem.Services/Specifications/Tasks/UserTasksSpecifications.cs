
using Domain.Contracts;
using Domain.Entities.TaskModels;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;

namespace Services.Specifications.Tasks
{
    public class UserTasksSpecifications : ProjectionSpecifications<TaskItem, TaskResultDto>
    {





        public UserTasksSpecifications(string userId, TaskParameters parameters) : base(

           Task => Task.CreatedByUserId == userId && Task.TaskType == TaskType.Personal &&
           (parameters.Priority == null || Task.Priority == parameters.Priority) &&
                   (parameters.Status == null || Task.Status == parameters.Status) &&
           (string.IsNullOrEmpty(parameters.Search) || Task.Title.ToLower().Contains(parameters.Search.ToLower())))
        {


            AddProjection(t => new TaskResultDto
            {
                Id = t.Id,
                Title = t.Title,
                Priority = t.Priority.ToString(),
                Status = t.Status.ToString(),
                DueDate = t.DueDate,
                AssignedToUserId = t.AssignedToUserId,


            });


            if (parameters.SortOptions != null)
            {



                switch (parameters.SortOptions)
                {


                    case TaskSortOptions.DueDateAsc:
                        SetOrderBy(T => T.DueDate ?? DateTime.MaxValue);
                        break;

                    case TaskSortOptions.DueDateDesc:
                        SetOrderByDescending(T => T.DueDate ?? DateTime.MaxValue);
                        break;

                    default:
                        SetOrderBy(T => T.Title ?? string.Empty); break;




                }

            }

            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }

    }
}
