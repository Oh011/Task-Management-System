using System.Text.Json.Serialization;

namespace Shared.Dtos.Tasks
{
    public class ProjectTaskDetailsDto : TaskDetailsDto
    {



        [JsonPropertyOrder(9)]
        public string? AssignedToName { get; set; }


        [JsonPropertyOrder(10)]

        public string? AssignedToProfileImageUrl { get; set; }
    }
}
