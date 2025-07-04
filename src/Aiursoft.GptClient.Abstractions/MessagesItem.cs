

using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class MessagesItem
{
    [JsonProperty("role")] public string? Role { get; set; }

    [JsonProperty("content")] public string? Content { get; set; }

    [JsonIgnore] public bool IsInjected { get; set; }

    public MessagesItem Clone()
    {
        return new MessagesItem
        {
            Role = Role,
            Content = Content,
            IsInjected = IsInjected
        };
    }
}
