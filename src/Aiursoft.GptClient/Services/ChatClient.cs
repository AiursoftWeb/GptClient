using System.Diagnostics;
using System.Text;
using Aiursoft.CSTools.Tools;
using Aiursoft.GptClient.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

    public virtual Task<HttpResponseMessage> AskStream(OpenAiRequestModel model, string completionApiUrl, string? token, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Asking LLM with model: {Model}， Endpoint: {Endpoint}.",
            model.Model,
            completionApiUrl);

        var json = JsonConvert.SerializeObject(model);
        var request = new HttpRequestMessage(HttpMethod.Post, completionApiUrl)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        if (token != null)
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        return response;
    }

    public virtual async Task<CompletionData> AskModel(OpenAiRequestModel model, string completionApiUrl, string? token, CancellationToken cancellationToken)
    {
        if (model.Stream == true)
        {
            throw new InvalidOperationException($"Stream is not supported in this method. Please use the {nameof(AskStream)} method.");
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var response = await AskStream(model, completionApiUrl, token, cancellationToken);
        try
        {
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            
            _logger.LogTrace("Received raw JSON response: {ResponseJson}", responseJson.SafeSubstring(5000));
            
            var responseModel = JsonConvert.DeserializeObject<CompletionDataInternal>(responseJson);
            
            try
            {
                responseModel!.FillChoices();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Both Choices and Message are empty"))
            {
                _logger.LogError("Failed to parse LLM response. Both Choices and Message are empty. " +
                    "Request last question: {Question}. Raw response: {RawResponse}",
                    model.Messages.LastOrDefault()?.Content?.SafeSubstring(2000),
                    responseJson.SafeSubstring(5000));
                throw;
            }

            _logger.LogTrace("Asked LLM. Request last question: {0}. Response last answer: {1}. Cost: {2}ms.",
                model.Messages.LastOrDefault()?.Content?.SafeSubstring(1500),
                responseModel.GetAnswerPart().SafeSubstring(1500),
                stopwatch.ElapsedMilliseconds);
            return responseModel;
        }
        catch (HttpRequestException raw)
        {
            var remoteError = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogError("Ask LLM failed. Request last question: {0}. Response last answer: {1}.",
                model.Messages.LastOrDefault()?.Content?.SafeSubstring(2000),
                remoteError.SafeSubstring(2000));
            throw new HttpRequestException(remoteError, raw);
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    public virtual async Task<CompletionData> AskString(string modelType, string completionApiUrl, string? token, string[] content, CancellationToken cancellationToken)
    {
        var model = new OpenAiRequestModel
        {
            Model = modelType,
            Messages = content.Select(x => new MessagesItem
            {
                Content = x,
                Role = "user"
            }).ToList()
        };
        return await AskModel(model, completionApiUrl, token, cancellationToken);
    }
}
