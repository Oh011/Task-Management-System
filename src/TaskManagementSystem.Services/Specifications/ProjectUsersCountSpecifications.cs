using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.ProjectModels;

namespace Services.Specifications
{
    class ProjectUsersCountSpecifications : GroupSpecification<ProjectUser, ProjectRole, ProjectUsersCount>
    {
        public ProjectUsersCountSpecifications(int projectId) : base(P => P.ProjectId == projectId)
        {

            AddGroupBy(p => p.Role);


            AddGroupSelector(p => new ProjectUsersCount
            {
                Role = p.Key,
                Count = p.Count()
            });


        }
    }


    public class ProjectUsersCount
    {

        public ProjectRole Role { get; set; }

        public int Count { get; set; } = 0;

    }
}
