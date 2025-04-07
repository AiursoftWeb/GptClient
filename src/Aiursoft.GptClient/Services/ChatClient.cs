using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Aiursoft.CSTools.Tools;
using Aiursoft.GptClient.Abstractions;
using Microsoft.Extensions.Logging;

namespace Aiursoft.GptClient.Services;

public class ChatClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public ChatClient(
        HttpClient httpClient,
        ILogger<ChatClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
        _logger = logger;
    }

    public virtual Task<HttpResponseMessage> AskStream(OpenAiModel model, string completionApiUrl, string? token)
    {
        _logger.LogInformation("Asking LLM with model: {Model}， Endpoint: {Endpoint}.",
            model.Model,
            completionApiUrl);

        var json = JsonSerializer.Serialize(model);
        var request = new HttpRequestMessage(HttpMethod.Post, completionApiUrl)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        if (token != null)
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        return response;
    }

    public virtual async Task<CompletionData> AskModel(OpenAiModel model, string completionApiUrl, string? token)
    {
        if (model.Stream == true)
        {
            throw new InvalidOperationException($"Stream is not supported in this method. Please use the {nameof(AskStream)} method.");
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var response = await AskStream(model, completionApiUrl, token);
        try
        {
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<CompletionDataInternal>(responseJson);
            responseModel!.FillChoices();

            _logger.LogInformation("Asked LLM. Request last question: {0}. Response last answer: {1}. Cost: {2}ms.",
                model.Messages.LastOrDefault()?.Content?.SafeSubstring(270),
                responseModel.GetAnswerPart().SafeSubstring(370),
                stopwatch.ElapsedMilliseconds);
            return responseModel;
        }
        catch (HttpRequestException raw)
        {
            var remoteError = await response.Content.ReadAsStringAsync();

            _logger.LogError("Ask LLM failed. Request last question: {0}. Response last answer: {1}.",
                model.Messages.LastOrDefault()?.Content?.SafeSubstring(270),
                remoteError.SafeSubstring(370));
            throw new HttpRequestException(remoteError, raw);
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    public virtual async Task<CompletionData> AskString(string modelType, string completionApiUrl, string? token, params string[] content)
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
        return await AskModel(model, completionApiUrl, token);
    }
}
