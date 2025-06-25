using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class OpenAiModel
{
    [JsonPropertyName("messages")] public List<MessagesItem> Messages { get; set; } = [];

    [JsonPropertyName("stream")] public bool? Stream { get; set; } = false;

    [JsonPropertyName("model")] public string? Model { get; set; } = string.Empty;

    [JsonPropertyName("temperature")] public double? Temperature { get; set; } = 0.5;

    [JsonPropertyName("presence_penalty")] public int? PresencePenalty { get; set; } = 0;

    [JsonPropertyName("tools")] public List<ToolsItem> Tools { get; set; } = [];

    public OpenAiModel Clone()
    {
        return new OpenAiModel
        {
            Messages = Messages.Select(m => m.Clone()).ToList(),
            Stream = Stream,
            Model = Model,
            Temperature = Temperature,
            PresencePenalty = PresencePenalty
        };
    }
}
