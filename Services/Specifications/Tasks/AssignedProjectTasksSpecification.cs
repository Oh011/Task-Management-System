using Domain.Contracts;
using Domain.Entities.TaskModels;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;

namespace Services.Specifications.Tasks
{
    internal class AssignedProjectTasksSpecification
   : ProjectionSpecifications<TaskItem, TaskResultDto>
    {
        public AssignedProjectTasksSpecification(int projectId, string userId, TaskParameters parameters)
            : base(t =>
                t.ProjectTask != null &&
                t.ProjectTask.ProjectId == projectId &&
                t.AssignedToUserId == userId &&


               (parameters.Priority == null || t.Priority == parameters.Priority) &&
                   (parameters.Status == null || t.Status == parameters.Status) &&
           (string.IsNullOrEmpty(parameters.Search) || t.Title.ToLower().Contains(parameters.Search.ToLower())))

        {


            AddProjection(t => new TaskResultDto
            {
                Id = t.Id,
                Title = t.Title,
                Priority = t.Priority.ToString(),
                Status = t.Status.ToString(),
                DueDate = t.DueDate


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
