using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Project
{
    public class ProjectUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }  // Project title


        [StringLength(1000)]
        public string? Description { get; set; }  // Optional description
    }
}
