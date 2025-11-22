using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Shared.Dtos;

namespace Services.Specifications.User
{
    internal class UserProfileSpecifications : ProjectionSpecifications<ApplicationUser, UserProfileDto>
    {
        public UserProfileSpecifications(string userId) : base(u => u.Id == userId && !u.IsDeleted)
        {

            AddInclude(u => u.AssignedTasks);


            AddProjection(u => new UserProfileDto
            {

                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                UserName = u.UserName,
                PhoneNumber = u.PhoneNumber,
                ProfileImageUrl = u.ProfileImageUrl,
                CreatedProjectCount = u.CreatedProjects.Count(),
                AssignedTaskCount = u.AssignedTasks.Count()
            });
        }
    }

}
