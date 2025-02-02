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
    
    public string ToModelString(GptModel gptModel)
    {
        return gptModel switch
        {
            GptModel.Gpt35Turbo => "gpt-3.5-turbo",
            GptModel.Gpt35Turbo16K => "gpt-3.5-turbo-16k",
            GptModel.Gpt4 => "gpt-4",
            GptModel.Gpt432K => "gpt-4-32k",
            GptModel.DeepseekR132B => "deepseek-r1:32b",
            _ => throw new ArgumentOutOfRangeException(nameof(gptModel), gptModel, null)
        };
    }

    public virtual async Task<CompletionData> AskModel(OpenAiModel model, GptModel gptModelType)
    {
        model.Model = ToModelString(gptModelType);
        _logger.LogInformation("Asking OpenAi with model: {Model}.", model.Model);

        var json = JsonSerializer.Serialize(model);
        _logger.LogInformation("Asking OpenAi with endpoint: {Endpoint}.", _completionApiUrl);
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
    
    public async Task<CompletionData> AskString(GptModel gptModelType, params string[] content)
    {
        var model = new OpenAiModel
        {
            Messages = content.Select(x => new MessagesItem
            {
                Content = x,
                Role = "user"
            }).ToList()
        };
        return await AskModel(model, gptModelType);
    }
}