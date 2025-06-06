using Domain.Entities.ProjectModels;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Project
{
    public class UpdateProjectStatus
    {

        [Required]
        public ProjectStatus ProjectStatus { get; set; }
    }
}
