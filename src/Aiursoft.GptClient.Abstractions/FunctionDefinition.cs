

using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class FunctionDefinition
{
    [JsonProperty("type")] public string? Type { get; set; }

    [JsonProperty("name")] public string? Name { get; set; }

    [JsonProperty("description")] public string? Description { get; set; }

    [JsonProperty("parameters")] public ParametersDefinition? Parameters { get; set; }
}
