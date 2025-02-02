using Aiursoft.GptClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.GptClient;

public static class Extensions
{
    /// <summary>
    /// Register git runners.
    /// 
    /// (If your project is using Aiursoft.Scanner, you do NOT have to call this!)
    /// </summary>
    /// <param name="services">Services to be injected.</param>
    /// <returns>The original services.</returns>
    public static IServiceCollection AddGptClient(this IServiceCollection services)
    {
        services.AddTransient<ChatClient>();
        return services;
    }
}
