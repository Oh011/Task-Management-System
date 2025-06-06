using Domain.Contracts;
using Domain.Entities.TaskModels;
using Shared.Dtos.Tasks;

namespace Services.Specifications.Tasks
{
    internal class ProjectTaskDetailsSpecifications : ProjectionSpecifications<TaskItem, ProjectTaskDetailsDto>
    {
        public ProjectTaskDetailsSpecifications(int id) : base(t => t.Id == id)
        {

            AddInclude(t => t.AssignedToUser);


            AddProjection(T => new ProjectTaskDetailsDto
            {

                Id = T.Id,
                Title = T.Title,
                Priority = T.Priority.ToString(),
                Status = T.Status.ToString(),
                DueDate = T.DueDate,
                AssignedToUserId = T.AssignedToUserId,
                AssignedToName = T.AssignedToUser.FullName,
                Description = T.Description,
                CreatedAt = T.CreatedAt,
                AssignedToProfileImageUrl = T.AssignedToUser.ProfileImageUrl,


            });
        }


    }
}
