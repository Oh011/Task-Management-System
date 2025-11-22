using System.Text.Json.Serialization;

namespace Shared.Dtos.Tasks
{
    public class TaskDetailsDto : TaskResultDto
    {



        [JsonPropertyOrder(5)]
        public string? Description { get; set; }

        [JsonPropertyOrder(6)]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyOrder(7)]
        public IEnumerable<string> AllowedStatus { get; set; }








    }




}
