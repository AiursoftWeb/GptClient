



using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class ToolsItem
{
    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("function")] public FunctionDefinition? Function { get; set; }
}
