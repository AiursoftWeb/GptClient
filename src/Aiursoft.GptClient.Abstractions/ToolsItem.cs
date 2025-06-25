using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class ToolsItem
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("function")] public FunctionDefinition? Function { get; set; }
}