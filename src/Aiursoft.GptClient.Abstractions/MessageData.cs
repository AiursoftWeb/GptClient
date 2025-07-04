

using Newtonsoft.Json;

namespace Aiursoft.GptClient.Abstractions;

public class MessageData
{
    /// <summary>
    /// The role of the message, such as "user" or "assistant".
    /// </summary>
    [JsonProperty("role")]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Role { get; set; }

    /// <summary>
    /// The content of the message.
    /// </summary>
    [JsonProperty("content")]
    public string? Content { get; set; }
}
