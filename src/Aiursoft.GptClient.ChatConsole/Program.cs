using Aiursoft.GptClient.Abstractions;
using Aiursoft.GptClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Aiursoft.GptClient.ChatConsole;

public abstract class Program
{
    private static string AskUser(string question, string? defaultValue, bool allowEmpty = false)
    {
        if (defaultValue != null)
        {
            Console.WriteLine($"{question} (For example: {defaultValue})");
        }
        else
        {
            Console.WriteLine(question);
        }

        var userInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(userInput))
        {
            if (!allowEmpty && defaultValue == null)
            {
                Console.WriteLine("Please enter a valid value.");
                return AskUser(question, defaultValue, allowEmpty);
            }

            userInput = defaultValue ?? "";
        }
        return userInput;
    }

    public static async Task Main(string[] args)
    {
        var endpoint = AskUser(
            """
            Please enter the chat API endpoint:
            
             * For ChatGPT, use https://api.openai.com/v1/chat/completions
             * For Ollma, use http://127.0.0.1:11434/api/chat
             * For other services, please enter the correct endpoint.
             
            """, 
            "http://127.0.0.1:11434/api/chat"
        );
        var apiKey = AskUser("Please enter the API key:", null, allowEmpty: true);
        var modelName = AskUser("Please enter the model name:", "DeepseekR132B");
        var model = Enum.Parse<GptModel>(modelName);
        
        var inMemorySettings = new Dictionary<string, string>
        {
            { "OpenAI:Token", apiKey },
            { "OpenAI:CompletionApiUrl", endpoint }
        };
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

        var history = new OpenAiModel();
        while (true)
        {
            var nextQuestion = AskUser("USER:", null);
            history.Messages.Add(new MessagesItem
            {
                Role = "user",
                Content = nextQuestion
            });

            var result = await chatClient.AskModel(history, model);
            Console.WriteLine("AI:");
            Console.WriteLine(result.GetAnswerPart());
            
            history.Messages.Add(new MessagesItem
            {
                Role = "assistant",
                Content = result.GetAnswerPart()
            });
        }
        
        // ReSharper disable once FunctionNeverReturns
    }
}