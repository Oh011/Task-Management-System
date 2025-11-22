namespace Shared.Dtos.Project
{
    public class ProjectSummaryDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public string Status { get; set; }
    }
}
