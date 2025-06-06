using System.Text.Json.Serialization;

namespace Domain.Entities.TaskModels
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskPriority
    {


        Low,
        Medium,
        High,
        Critical
    }
}


//[JsonConverter(typeof(JsonStringEnumConverter))]:
//Deserialization: When you send an enum value in a request body (e.g., in a POST or PUT request), 
//ASP.NET Core will now correctly interpret the enum name (like "Low", "Medium") instead of just using the integer value.