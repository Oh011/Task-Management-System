using Domain.Contracts;
using Domain.Entities.TaskModels;
using Shared.Dtos.Tasks;
using Shared.ParameterTypes;

namespace Services.Specifications.Tasks
{
    internal class ProjectTasksSpecifications : ProjectionSpecifications<TaskItem, ProjectTaskResultDto>
    {




        public ProjectTasksSpecifications(int id, TaskParameters parameters) :
            base(Task => (Task.ProjectTask != null && Task.ProjectTask.ProjectId == id) &&
                (parameters.Priority == null || Task.Priority == parameters.Priority) &&
                (parameters.Status == null || Task.Status == parameters.Status) &&
            (string.IsNullOrEmpty(parameters.Search) || Task.Title.ToLower().Contains(parameters.Search.ToLower())))


        {

            AddInclude(T => T.AssignedToUser);

            AddProjection(T => new ProjectTaskResultDto
            {

                Id = T.Id,
                Title = T.Title,
                Priority = T.Priority.ToString(),
                Status = T.Status.ToString(),
                DueDate = T.DueDate,
                AssignedToUserId = T.AssignedToUserId,
                AssignedToName = T.AssignedToUser.FullName,
                AssignedToProfileImageUrl = T.AssignedToUser.ProfileImageUrl,


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
