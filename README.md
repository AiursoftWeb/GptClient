# Aiursoft GptClient

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.com/aiursoft/gptClient/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.com/aiursoft/gptClient/badges/master/pipeline.svg)](https://gitlab.aiursoft.com/aiursoft/gptClient/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.com/aiursoft/gptClient/badges/master/coverage.svg)](https://gitlab.aiursoft.com/aiursoft/gptClient/-/pipelines)
[![NuGet version (Aiursoft.GptClient)](https://img.shields.io/nuget/v/Aiursoft.GptClient.svg)](https://www.nuget.org/packages/Aiursoft.GptClient/)
[![NuGet version (Aiursoft.GptClient.ChatConsole)](https://img.shields.io/nuget/v/Aiursoft.GptClient.ChatConsole.svg)](https://www.nuget.org/packages/Aiursoft.GptClient.ChatConsole/)
[![ManHours](https://manhours.aiursoft.com/r/gitlab.aiursoft.com/aiursoft/GptClient.svg)](https://gitlab.aiursoft.com/aiursoft/GptClient/-/commits/master?ref_type=heads)

The SDK for ChatGpt. Simple implementation and easy to use.

## How to install

First, install `Aiursoft.GptClient` to your ASP.NET Core project from nuget.org:

```bash
dotnet add package Aiursoft.GptClient
```

Exposed API in `ChatClient`:

```csharp
public Task<CompletionData> AskModel(OpenAiModel model);
public Task<CompletionData> AskString(string gptModelType, params string[] content);
```

Required IConfiguration keys:

```json
{
    "OpenAI:Token": "YourOpenAIKey",
    "OpenAI:CompletionApiUrl": "https://api.openai.com/v1/engines/davinci/completions"
}
```

For example, you can use the following code to create a simple ChatGpt client:

```csharp
using Aiursoft.GptClient;
using Aiursoft.GptClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Test
{
    public abstract class Program
    {
        public static async Task Main()
        {
            // Keys
            var apiKey = "";
            var endpoint = "http://localhost:11434/api/chat";
            var model = "qwen3:30b-a3b-thinking-2507-q8_0";

            // Create a simple ChatGpt client.
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddLogging(logging => { logging.SetMinimumLevel(LogLevel.Warning); });
            services.AddGptClient();
            var serviceProvider = services.BuildServiceProvider();
            var chatClient = serviceProvider.GetRequiredService<ChatClient>();
            async Task<string> GetAnswer(string[] prompts)
            {
                return (await chatClient.AskString(
                    modelType: model,
                    completionApiUrl: endpoint,
                    token: apiKey,
                    content: prompts,
                    CancellationToken.None)).GetAnswerPart();
            }

            // Example usage
            var answer = await GetAnswer(["Why is the sky blue?"]);
            Console.WriteLine(answer);
        }
    }
}
```

Now you have built a simple ChatGpt client.

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
