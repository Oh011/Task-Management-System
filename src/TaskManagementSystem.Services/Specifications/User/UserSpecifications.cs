using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Shared.Dtos;
using Shared.ParameterTypes;

namespace Services.Specifications.User
{
    public class UserSpecifications : ProjectionSpecifications<ApplicationUser, UserInfoDto>
    {


        public UserSpecifications(string id) : base(u => u.Id == id)
        {


            AddProjection(u => new UserInfoDto
            {

                UserName = u.UserName,
                Email = u.Email,
                ProfileImageUrl = u.ProfileImageUrl,
                Id = u.Id,
            });



        }


        public UserSpecifications(UserSearchParameters parameters)
    : base(u =>
        string.IsNullOrEmpty(parameters.Query) ||
         u.UserName.Contains(parameters.Query) ||
         u.Email.Contains(parameters.Query)
    )
        {

            AddProjection(u => new UserInfoDto
            {

                UserName = u.UserName,
                Email = u.Email,
                ProfileImageUrl = u.ProfileImageUrl,
                Id = u.Id,
            });

            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }

        public UserSpecifications(int projectId, UserSearchParameters parameters)
    : base(u =>
        (string.IsNullOrEmpty(parameters.Query) ||
         u.UserName.Contains(parameters.Query) ||
         u.Email.Contains(parameters.Query)) &&
        !u.Projects.Any(pu => pu.ProjectId == projectId) // exclude users already assigned to project
    )
        {
            AddProjection(u => new UserInfoDto
            {

                UserName = u.UserName,
                Email = u.Email,
                ProfileImageUrl = u.ProfileImageUrl,
                Id = u.Id,
            });


            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }



    }
}
