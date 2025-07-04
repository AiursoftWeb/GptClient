using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class OpenAiRequestModel
{
    [JsonPropertyName("messages")] public List<MessagesItem> Messages { get; set; } = [];

    [JsonPropertyName("stream")] public bool? Stream { get; set; } = false;

    [JsonPropertyName("model")] public string? Model { get; set; } = string.Empty;

    [JsonPropertyName("temperature")] public double? Temperature { get; set; } = 0.5;

    [JsonPropertyName("presence_penalty")] public int? PresencePenalty { get; set; } = 0;

    public OpenAiRequestModel CloneAsOpenAiRequestModel()
    {
        return new OpenAiRequestModel
        {
            Messages = Messages.Select(m => m.Clone()).ToList(),
            Stream = Stream,
            Model = Model,
            Temperature = Temperature,
            PresencePenalty = PresencePenalty
        };
    }
}

public class OllamaRequestModel : OpenAiRequestModel
{
    [JsonPropertyName("tools")] public List<ToolsItem> Tools { get; set; } = [];

    public OllamaRequestModel CloneAsOllamaRequestModel()
    {
        return new OllamaRequestModel
        {
            Messages = Messages.Select(m => m.Clone()).ToList(),
            Stream = Stream,
            Model = Model,
            Temperature = Temperature,
            PresencePenalty = PresencePenalty,
            Tools = Tools.ToList()
        };
    }
}
