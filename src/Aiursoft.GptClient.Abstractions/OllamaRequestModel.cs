using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class OpenAiRequestModel
{
    [JsonProperty("messages")] public List<MessagesItem> Messages { get; set; } = [];

    [JsonProperty("stream")] public bool? Stream { get; set; } = false;

    [JsonProperty("model")] public string? Model { get; set; } = string.Empty;

    [JsonProperty("temperature")] public double? Temperature { get; set; } = 0.5;

    [JsonProperty("presence_penalty")] public int? PresencePenalty { get; set; } = 0;

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
    [JsonProperty("tools")] public List<ToolsItem> Tools { get; set; } = [];

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
