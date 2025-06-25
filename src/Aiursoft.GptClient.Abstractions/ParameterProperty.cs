using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class ParameterProperty
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }
}