
using Domain.Entities.IdentityModels;
using Domain.Entities.ProjectModels;
using Domain.Entities.TaskModels;

namespace Domain.Entities
{
    public class ProjectUser : BaseEntity<int>
    {

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }


        public int? ProjectId { get; set; }

        public Project? Project { get; set; }


        public ProjectRole Role { get; set; } = ProjectRole.Member;
    }
}
