using System.Text.Json.Serialization;

namespace Shared.Dtos.Tasks
{
    public class TaskResultDto
    {

        [JsonPropertyOrder(0)]
        public new int Id { get; set; }

        [JsonPropertyOrder(1)]
        public new string Title { get; set; } = string.Empty;

        [JsonPropertyOrder(2)]
        public new string Priority { get; set; }

        [JsonPropertyOrder(3)]
        public new string Status { get; set; }

        [JsonPropertyOrder(4)]
        public new DateTime? DueDate { get; set; }

        [JsonPropertyOrder(8)]
        public string? AssignedToUserId { get; set; }


    }
}

//var specifications = new UserTaskSpecification(userId, parameters);
//var result = await GetTasksAsync(specifications, userId, parameters);



//return new PaginatedResult<TaskResultDto>(

//    result.PageIndex,
//    result.PageSize,
//    result.TotalCount,
//    _mapper.Map<IEnumerable<TaskResultDto>>(result.Items)

//    );