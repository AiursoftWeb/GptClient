using System.Text.Json.Serialization;

namespace Aiursoft.GptClient.Abstractions;

public class CompletionData
{
    /// <summary>
    /// The ID of the completion.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The type of the object, which is always "text_completion".
    /// </summary>
    [JsonPropertyName("object")]
    public string? Object { get; set; }

    /// <summary>
    /// The timestamp when the completion was created.
    /// </summary>
    [JsonPropertyName("created")]
    public int? Created { get; set; }

    /// <summary>
    /// The name of the model used to generate the completion.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    /// The usage data for this completion.
    /// </summary>
    [JsonPropertyName("usage")]
    public UsageData? Usage { get; set; }

    /// <summary>
    /// The list of choices generated by the completion.
    ///
    /// For library users: Do NOT access neither Choices nor Message directly. Use GetContent() and SetContent() instead.
    /// </summary>
    [JsonPropertyName("choices")]
    // ReSharper disable once CollectionNeverUpdated.Global
    // ReSharper disable once CollectionNeverQueried.Global
    public List<ChoicesItemData>? Choices { get; set; } = [];

    public string GetFullContent()
    {
        return Choices!.First().Message?.Content ?? string.Empty;
    }

    public void SetContent(string content)
    {
        Choices =
        [
            new ChoicesItemData
            {
                Message = new MessageData
                {
                    Content = content,
                    Role = "assistant"
                }
            }
        ];
    }

    public string GetThinkPart()
    {
        var content = GetFullContent();
        const string startTag = "<think>";
        const string endTag = "</think>";
        var startIdx = content.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);
        var endIdx = content.IndexOf(endTag, StringComparison.OrdinalIgnoreCase);

        // Found think part, return the content between <think> and </think>
        if (startIdx != -1 && endIdx != -1 && endIdx > startIdx)
        {
            var thinkStart = startIdx + startTag.Length;
            var thinkPart = content.Substring(thinkStart, endIdx - thinkStart);
            return thinkPart.Trim();
        }

        // Return empty string if no think part found
        return string.Empty;
    }

    public string GetAnswerPart()
    {
        var content = GetFullContent();
        const string startTag = "<think>";
        const string endTag = "</think>";
        var startIdx = content.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);
        var endIdx = content.IndexOf(endTag, StringComparison.OrdinalIgnoreCase);

        // If think part exists, the actual answer is the content after the think tag
        if (startIdx != -1 && endIdx != -1 && endIdx > startIdx)
        {
            var answerStart = endIdx + endTag.Length;
            return content.Substring(answerStart).Trim();
        }

        // If no think tag found, return the whole content as is
        return content;
    }
}

public class CompletionDataInternal : CompletionData
{
    /// <summary>
    /// For some API versions, the message is returned directly in the completion object.
    ///
    /// For library users: Do NOT access neither Choices nor Message directly. Use GetContent() and SetContent() instead.
    /// </summary>
    [JsonPropertyName("message")]
    public MessageData? Message { get; set; }

    public void FillChoices()
    {
        if ((Choices == null || Choices.Count == 0 || string.IsNullOrEmpty(Choices.First().Message?.Content)) && Message?.Content != null)
        {
            Choices ??=
            [
                new ChoicesItemData()
                {
                    Message = new MessageData
                    {
                        Content = Message.Content,
                        Role = "assistant"
                    }
                }
            ];
        }
    }
}
