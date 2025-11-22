using Domain.Entities.IdentityModels;
using Domain.Entities.TaskModels;

namespace Domain.Entities.ProjectModels
{
    public class Project : BaseEntity<int>
    {

        public string Name { get; set; } = string.Empty;  // Project title

        public string? Description { get; set; }  // Optional description

        public DateTime StartDate { get; set; }  // When project starts




        public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;  // Project lifecycle status


        // Navigation properties
        //public User? OwnerUser { get; set; }  // The project owner


        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        public IEnumerable<ProjectUser>? ProjectUsers { get; set; }
        public IEnumerable<ProjectTask>? ProjectTasks { get; set; }


        public IEnumerable<ProjectInvitation>? ProjectInvitations { get; set; }

        //public ICollection<ProjectMember>? Members { get; set; }  // Users added to the project
    }
}
