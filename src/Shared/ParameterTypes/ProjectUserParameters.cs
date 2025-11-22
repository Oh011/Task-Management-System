using Domain.Entities.ProjectModels;

namespace Shared.ParameterTypes
{
    public class ProjectUserParameters : PaginationQueryParameters
    {
        public string? Name { get; set; }


        public ProjectRole? Role { get; set; }
    }
}
