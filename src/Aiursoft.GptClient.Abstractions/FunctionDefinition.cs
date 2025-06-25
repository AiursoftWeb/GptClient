using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class FunctionDefinition
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("parameters")] public ParametersDefinition? Parameters { get; set; }
}