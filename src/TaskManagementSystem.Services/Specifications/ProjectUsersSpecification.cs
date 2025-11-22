using Domain.Contracts;
using Domain.Entities;
using Shared.Dtos.Project;
using Shared.ParameterTypes;

namespace Services.Specifications
{
    public class ProjectUsersSpecification : ProjectionSpecifications<ProjectUser, ProjectUserDto>
    {


        public ProjectUsersSpecification(int projectId, string userId)
        : base(PU => PU.ProjectId == projectId && PU.UserId == userId)
        {

            AddInclude(pu => pu.User);

            AddProjection(pu => new ProjectUserDto
            {
                UserId = pu.UserId,
                Email = pu.User.Email,
                UserName = pu.User.UserName,
                Role = pu.Role.ToString(),
                ProfileImageUrl = pu.User.ProfileImageUrl
            });

        }



        public ProjectUsersSpecification(int projectId, ProjectUserParameters parameters)
     : base(PU =>

         PU.ProjectId == projectId &&

         (string.IsNullOrEmpty(parameters.Name) ||
          (PU.User.UserName.ToLowerInvariant().Contains(parameters.Name.ToLowerInvariant())

        ||

        PU.User.Email.ToLowerInvariant().Contains(parameters.Name.ToLowerInvariant())


              ))
     &&
         (!parameters.Role.HasValue || PU.Role == parameters.Role)
     )
        {
            AddInclude(pu => pu.User);

            AddProjection(pu => new ProjectUserDto
            {
                UserId = pu.UserId,
                Email = pu.User.Email,
                UserName = pu.User.UserName,
                Role = pu.Role.ToString(),
                ProfileImageUrl = pu.User.ProfileImageUrl
            });

            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }
    }
}
