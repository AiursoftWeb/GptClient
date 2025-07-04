


using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class ParametersDefinition
{
    [JsonProperty("type")] public string? Type { get; set; }

    /// <summary>
    /// 动态的 property 名称都能被映射到这里：
    /// 比如 "timezone" / "source_timezone" / "time" / "target_timezone" 等
    /// </summary>
    [JsonProperty("properties")]
    public Dictionary<string, ParameterProperty> Properties { get; set; } = new();

    [JsonProperty("required")] public List<string> Required { get; set; } = [];
}
