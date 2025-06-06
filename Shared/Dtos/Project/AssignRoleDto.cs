using Domain.Entities.ProjectModels;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Project
{
    public class AssignRoleDto
    {
        [Required]
        public string TargetUserId { get; set; } = default!;


        [Required]
        public ProjectRole NewRole { get; set; }
    }
}
