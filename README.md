# Aiursoft GptClient

<a name="aiursoft-gptclient"></a>

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/AiursoftWeb/gptclient/blob/master/LICENSE)
[![Pipeline stat](https://badge-proxy.aiursoft.com/aiursoft/gptClient/badges/master/pipeline.svg)](https://gitlab.aiursoft.com/aiursoft/gptClient/-/pipelines)
[![Test Coverage](https://badge-proxy.aiursoft.com/aiursoft/gptClient/badges/master/coverage.svg)](https://gitlab.aiursoft.com/aiursoft/gptClient/-/pipelines)
[![S360 compliance](https://s360.aiursoft.com/badge.svg?project=aiursoft%2Fgptclient)](https://s360.aiursoft.com/SupplyChain?tab=projects&project=aiursoft%2Fgptclient&scope=baseline&ecosystem=all)
[![NuGet version (Aiursoft.GptClient)](https://img.shields.io/nuget/v/Aiursoft.GptClient.svg)](https://www.nuget.org/packages/Aiursoft.GptClient/)
[![NuGet version (Aiursoft.GptClient.ChatConsole)](https://img.shields.io/nuget/v/Aiursoft.GptClient.ChatConsole.svg)](https://www.nuget.org/packages/Aiursoft.GptClient.ChatConsole/)
[![Man hours](https://manhours.aiursoft.com/r/github.com/aiursoftweb/GptClient.svg)](https://manhours.aiursoft.com/r/github.com/aiursoftweb/GptClient.html)

The SDK for ChatGpt. Simple implementation and easy to use.

## Overview

### What is GptClient?

A .NET HTTP client for model servers, with typed requests, streaming and non-streaming responses, and per-request endpoint configuration.

## Installation

Add the package to your .NET project:

<a name="how-to-install"></a>

First, install `Aiursoft.GptClient` to your ASP.NET Core project from nuget.org:

```bash
dotnet add package Aiursoft.GptClient
```

**Standalone console example**

The console example also uses the standard Microsoft service registrations:

```bash
dotnet add package Microsoft.Extensions.Http
```

Requires .NET 10 or later.

## Usage

Configure an accessible model server before running this complete console example:

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
            var model = "qwen3:32b-q8_0";

            // Create a simple ChatGpt client.
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddLogging(logging => { logging.SetMinimumLevel(LogLevel.Warning); });
            services.AddGptClient();
            using var serviceProvider = services.BuildServiceProvider();
            var chatClient = serviceProvider.GetRequiredService<ChatClient>();
            async Task<string> GetAnswer(string[] prompts)
            {
                return (await chatClient.AskString(
                    modelType: model,
                    completionApiUrl: endpoint,
                    token: apiKey,
                    content: prompts,
                    cancellationToken: CancellationToken.None)).GetAnswerPart();
            }

            // Example usage
            var answer = await GetAnswer(["Why is the sky blue?"]);
            Console.WriteLine(answer);
        }
    }
}
```

Now you have built a simple ChatGpt client.

### API reference

Exposed API in `ChatClient`:

```csharp
public Task<CompletionData> AskModel(OpenAiRequestModel model, string completionApiUrl, string? token, CancellationToken cancellationToken);
public Task<CompletionData> AskString(string modelType, string completionApiUrl, string? token, string[] content, CancellationToken cancellationToken);
```

## Configuration

**Endpoint, model and token**

Pass `modelType`, `completionApiUrl`, `token`, prompt content and a cancellation token explicitly, as in the Usage example. `ChatClient` does not read `IConfiguration` keys automatically. A null token omits the Authorization header; a non-null value is sent as a Bearer token. Choose an endpoint and request/response format supported by the deployed model server.

The consuming application may keep its own settings in this shape and pass the values to the client:

```json
{
    "OpenAI:Token": "YourOpenAIKey",
    "OpenAI:CompletionApiUrl": "http://localhost:11434/api/chat"
}
```

These are application-owned keys, not required library settings. The local-server example uses the endpoint and model shown in Usage. `ChatClient` sets its HTTP timeout to five minutes; cancellation is also passed per request.

## Development

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

From the repository root, restore, build and test:

```bash
dotnet restore Aiursoft.GptClient.sln
dotnet build Aiursoft.GptClient.sln --no-restore
dotnet test Aiursoft.GptClient.sln --no-restore
```

Run `./lint.sh` for the existing static checks.

## How to contribute

<a name="how-to-contribute"></a>

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you have push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.

## License

Licensed under the [MIT License](https://github.com/AiursoftWeb/gptclient/blob/master/LICENSE).
