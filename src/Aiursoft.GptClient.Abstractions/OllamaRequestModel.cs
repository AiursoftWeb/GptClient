using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class OpenAiRequestModel
{
    [JsonProperty("messages")] public List<MessagesItem> Messages { get; set; } = [];

    [JsonProperty("stream")] public bool? Stream { get; set; } = false;

    [JsonProperty("model")] public string? Model { get; set; } = string.Empty;

    [JsonProperty("temperature")] public double? Temperature { get; set; } = 0.5;

    [JsonProperty("presence_penalty")] public int? PresencePenalty { get; set; } = 0;

    [JsonProperty("response_format")] public ResponseFormat? ResponseFormat { get; set; }

    public OpenAiRequestModel CloneAsOpenAiRequestModel()
    {
        return new OpenAiRequestModel
        {
            Messages = Messages.Select(m => m.Clone()).ToList(),
            Stream = Stream,
            Model = Model,
            Temperature = Temperature,
            PresencePenalty = PresencePenalty,
            ResponseFormat = ResponseFormat?.Clone()
        };
    }
}

public class OllamaRequestOptions
{
    [JsonProperty("num_ctx")]
    public int? NumCtx { get; set; }

    [JsonProperty("temperature")]
    public float? Temperature { get; set; }

    [JsonProperty("top_p")]
    public float? TopP { get; set; }

    [JsonProperty("top_k")]
    public int? TopK { get; set; }
    
    [JsonProperty("num_predict")]
    public int? NumPredict { get; set; }
    
    [JsonProperty("repeat_penalty")]
    public float? RepeatPenalty { get; set; }
    
    [JsonProperty("stop")]
    public string[]? Stop { get; set; }

    public OllamaRequestOptions Clone()
    {
        return new OllamaRequestOptions
        {
            NumCtx = NumCtx,
            Temperature = Temperature,
            TopP = TopP,
            TopK = TopK,
            NumPredict = NumPredict,
            RepeatPenalty = RepeatPenalty,
            Stop = Stop?.ToArray()
        };
    }
}

public class OllamaRequestModel : OpenAiRequestModel
{
    [JsonProperty("tools")] public List<ToolsItem> Tools { get; set; } = [];
    
    [JsonProperty("options")] public OllamaRequestOptions? Options { get; set; }

    [JsonProperty("think")] public bool? Think { get; set; }

    public OllamaRequestModel CloneAsOllamaRequestModel()
    {
        return new OllamaRequestModel
        {
            Messages = Messages.Select(m => m.Clone()).ToList(),
            Stream = Stream,
            Model = Model,
            Temperature = Temperature,
            PresencePenalty = PresencePenalty,
            Tools = Tools.ToList(),
            ResponseFormat = ResponseFormat?.Clone(),
            Options = Options?.Clone(),
            Think = Think
        };
    }
}
