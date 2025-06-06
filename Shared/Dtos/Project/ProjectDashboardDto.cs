namespace Shared.Dtos.Project
{
    public class ProjectDashboardDto
    {

        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;

        public string Status { get; set; }
        public DateTime StartDate { get; set; }



        public Dictionary<string, int>? TaskCountsByStatus { get; set; }

        public double DonePercentage { get; set; }
        public double InProgressPercentage { get; set; }
        public double ToDoPercentage { get; set; }
        public double BlockedPercentage { get; set; }


        public int TotalTasks { get; set; }
        // Member summary

        public Dictionary<string, int>? MemberCountsByRole { get; set; }


        public int TotalMembers { get; set; }

    }
}
