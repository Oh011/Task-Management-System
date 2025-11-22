using Domain.Entities.ProjectModels;

namespace Shared.Dtos.Project
{
    public class ProjectResultDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // Project title

        public string? Description { get; set; }  // Optional description

        public DateTime StartDate { get; set; }  // When project starts


        public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;  // Project lifecycle status


        public string UserRole { get; set; } = string.Empty;
        public Dictionary<string, Dictionary<string, bool>> AllowedActions { get; set; }



        public IEnumerable<string>? AllowedStatus { get; set; }

    }
}
