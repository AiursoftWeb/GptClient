using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Aiursoft.CSTools.Tools;
using Aiursoft.GptClient.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Aiursoft.GptClient.Services;

public class ChatClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string? _token;
    private readonly string _completionApiUrl;

    public ChatClient(
        HttpClient httpClient,
        ILogger<ChatClient> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
        _logger = logger;
        _token = configuration["OpenAI:Token"]!;
        _completionApiUrl = configuration["OpenAI:CompletionApiUrl"]!;
    }

    public virtual async Task<CompletionData> AskModel(OpenAiModel model)
    {
        _logger.LogInformation("Asking OpenAi with model: {Model}， Endpoint: {Endpoint}.",
            model.Model,
            _completionApiUrl);

        var json = JsonSerializer.Serialize(model);
        var request = new HttpRequestMessage(HttpMethod.Post, _completionApiUrl)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        if (_token != null)
        {
            request.Headers.Add("Authorization", $"Bearer {_token}");
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var response = await _httpClient.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<CompletionData>(responseJson);

            _logger.LogInformation("Asked OpenAi. Request last question: {0}. Response last answer: {1}. Cost: {2}ms.",
                model.Messages.LastOrDefault()?.Content?.SafeSubstring(70),
                responseModel?.GetAnswerPart().SafeSubstring(70),
                stopwatch.ElapsedMilliseconds);
            return responseModel!;
        }
        catch (HttpRequestException raw)
        {
            var remoteError = await response.Content.ReadAsStringAsync();

            _logger.LogError("Asked OpenAi failed. Request last question: {0}. Response last answer: {1}.",
                model.Messages.LastOrDefault()?.Content?.SafeSubstring(70),
                remoteError.SafeSubstring(70));
            throw new HttpRequestException(remoteError, raw);
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    public virtual async Task<CompletionData> AskString(string modelType, params string[] content)
    {
        var model = new OpenAiModel
        {
            Model = modelType,
            Messages = content.Select(x => new MessagesItem
            {
                Content = x,
                Role = "user"
            }).ToList()
        };
        return await AskModel(model);
    }
}
