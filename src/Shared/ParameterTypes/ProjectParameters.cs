using System.ComponentModel;

namespace Shared.ParameterTypes
{
    public class ProjectParameters : PaginationQueryParameters
    {

        public string? Name { get; set; } = string.Empty;  // Project title



        public ProjectSortOptions? SortOptions { get; set; }  // When project starts


        public bool? includeTasks { get; set; } = false;
    }



    public enum ProjectSortOptions
    {


        [Description("Sort by DueDate Ascending")]
        StartDateAsc,

        [Description("Sort by DueDate Descending")]
        StartDateDesc,
    }
}
