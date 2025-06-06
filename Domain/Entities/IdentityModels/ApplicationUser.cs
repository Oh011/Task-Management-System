using Domain.Entities.ProjectModels;
using Domain.Entities.TaskModels;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }

        // Store the profile image as a URL (e.g., /images/users/avatar.png)
        public string? ProfileImageUrl { get; set; }

        // Navigation Properties
        public ICollection<TaskItem>? CreatedTasks { get; set; }
        public ICollection<Project>? CreatedProjects { get; set; }


        public IEnumerable<ProjectUser>? Projects { get; set; }

        public ICollection<TaskItem>? AssignedTasks { get; set; }

        public IEnumerable<ProjectInvitation>? ProjectInvitations { get; set; }
        public bool IsDeleted { get; set; } = false;




    }
}
