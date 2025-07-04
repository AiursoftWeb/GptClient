



using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class ParameterProperty
{
    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("description")] public string? Description { get; set; }
}
