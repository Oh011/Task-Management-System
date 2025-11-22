using System.Text.Json.Serialization;

namespace Shared.Dtos.Tasks
{
    public class ProjectTaskResultDto : TaskResultDto
    {

        [JsonPropertyOrder(5)]
        public string? AssignedToName { get; set; }

        [JsonPropertyOrder(6)]
        public string? AssignedToProfileImageUrl { get; set; }
    }
}
