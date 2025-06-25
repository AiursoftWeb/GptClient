using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class ParametersDefinition
{
    [JsonPropertyName("type")] public string? Type { get; set; }

    /// <summary>
    /// 动态的 property 名称都能被映射到这里：
    /// 比如 "timezone" / "source_timezone" / "time" / "target_timezone" 等
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, ParameterProperty> Properties { get; set; } = new();

    [JsonPropertyName("required")] public List<string> Required { get; set; } = [];
}
