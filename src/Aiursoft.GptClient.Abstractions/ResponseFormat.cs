using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class ResponseFormat
{
    [JsonProperty("type")] public string? Type { get; set; }

    public ResponseFormat Clone()
    {
        return new ResponseFormat
        {
            Type = Type
        };
    }
}
