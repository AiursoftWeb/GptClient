# Aiursoft GptClient

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/aiursoft/gptClient/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/aiursoft/gptClient/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/aiursoft/gptClient/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/aiursoft/gptClient/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/aiursoft/gptClient/-/pipelines)
[![NuGet version (Aiursoft.GptClient)](https://img.shields.io/nuget/v/Aiursoft.GptClient.svg)](https://www.nuget.org/packages/Aiursoft.GptClient/)
[![NuGet version (Aiursoft.GptClient.ChatConsole)](https://img.shields.io/nuget/v/Aiursoft.GptClient.ChatConsole.svg)](https://www.nuget.org/packages/Aiursoft.GptClient.ChatConsole/)
[![ManHours](https://manhours.aiursoft.cn/r/gitlab.aiursoft.cn/aiursoft/GptClient.svg)](https://gitlab.aiursoft.cn/aiursoft/GptClient/-/commits/master?ref_type=heads)

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
var inMemorySettings = new Dictionary<string, string>
{
    { "OpenAI:Token", apiKey },
    { "OpenAI:CompletionApiUrl", endpoint }
};
var model = GptModel.DeepseekR170B;
var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(inMemorySettings!)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddHttpClient();
services.AddLogging(logging =>
{
    logging.SetMinimumLevel(LogLevel.Warning);
});
services.AddGptClient();
var serviceProvider = services.BuildServiceProvider();
var chatClient = serviceProvider.GetRequiredService<ChatClient>();

var history = new OpenAiModel
{
    Model = modelName
};
while (true)
{
    var nextQuestion = AskUser("USER:", null);
    history.Messages.Add(new MessagesItem
    {
        Role = "user",
        Content = nextQuestion
    });

    var result = await chatClient.AskModel(history);
    Console.WriteLine("AI:");
    Console.WriteLine(result.GetAnswerPart());
    
    history.Messages.Add(new MessagesItem
    {
        Role = "assistant",
        Content = result.GetAnswerPart()
    });
}
```

Now you have built a simple ChatGpt client.

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
