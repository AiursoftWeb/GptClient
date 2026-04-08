using System.Text;
using Aiursoft.GptClient.Abstractions;
using Aiursoft.GptClient.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Aiursoft.GptClient.Tests;

[TestClass]
public class StreamingTests
{
    private class MockHttpMessageHandler(string[] chunks) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            foreach (var chunk in chunks)
            {
                var data = new CompletionDataInternal();
                data.Choices =
                [
                    new ChoicesItemData
                    {
                        Delta = new MessageData
                        {
                            Content = chunk,
                            Role = "assistant"
                        }
                    }
                ];
                sb.Append("data: " + JsonConvert.SerializeObject(data) + "\n\n");
            }
            sb.Append("data: [DONE]\n\n");

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(sb.ToString())
            };
            return Task.FromResult(response);
        }
    }

    [TestMethod]
    public async Task TestAskModelStream()
    {
        var chunks = new[] { "Hello", " ", "world", "!" };
        var handler = new MockHttpMessageHandler(chunks);
        var httpClient = new HttpClient(handler);
        var loggerMock = new Mock<ILogger<ChatClient>>();
        var client = new ChatClient(httpClient, loggerMock.Object);

        var model = new OpenAiRequestModel
        {
            Model = "test-model",
            Messages = [new MessagesItem { Content = "Hi", Role = "user" }]
        };

        var result = new List<string>();
        await foreach (var part in client.AskModelStream(model, "http://localhost", "token", default))
        {
            result.Add(part);
        }

        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("Hello", result[0]);
        Assert.AreEqual(" ", result[1]);
        Assert.AreEqual("world", result[2]);
        Assert.AreEqual("!", result[3]);
    }
}
