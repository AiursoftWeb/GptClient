using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class OpenAiModel
{
    [JsonPropertyName("messages")] public List<MessagesItem> Messages { get; set; } = new();

    [JsonPropertyName("stream")] public bool? Stream { get; set; } = false;

    /// <summary>
    /// This model is not used in the current implementation.
    ///
    /// Keep here for some API projects might need JSON deserialization to correctly deserialize the input.
    /// </summary>
    [JsonPropertyName("model")] public string? Model { get; set; } = "gpt-4-0613";

    [JsonPropertyName("temperature")] public double? Temperature { get; set; } = 0.5;

    [JsonPropertyName("presence_penalty")] public int? PresencePenalty { get; set; } = 0;

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