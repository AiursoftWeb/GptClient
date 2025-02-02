# Aiursoft GptClient

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/aiursoft/gptClient/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/aiursoft/gptClient/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/aiursoft/gptClient/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/aiursoft/gptClient/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/aiursoft/gptClient/-/pipelines)
[![NuGet version (Aiursoft.GptClient)](https://img.shields.io/nuget/v/Aiursoft.GptClient.svg)](https://www.nuget.org/packages/Aiursoft.GptClient/)
[![ManHours](https://manhours.aiursoft.cn/r/gitlab.aiursoft.cn/aiursoft/GptClient.svg)](https://gitlab.aiursoft.cn/aiursoft/GptClient/-/commits/master?ref_type=heads)

An Automatic dependencies management system for ASP.NET Core and powers Aiursoft.

## Why this project

The traditional way to add dependencies is:

```csharp
service.AddScoped<MyScopedDependency>();
```

Which means that you have to manually inject all dependencies. When you have too many of them, it is possible to make a mistake.

## How to use Aiursoft.GptClient

First, install `Aiursoft.GptClient` to your ASP.NET Core project from nuget.org:

```bash
dotnet add package Aiursoft.GptClient
```

Add the interface to your class like this:

```csharp
using Aiursoft.GptClient.Abstractions;

public class MySingletonService : ISingletonDependency
{

}

public class MyScopedService : IScopedDependency
{

}

public class MyTransientService : ITransientDependency
{

}
```

And just call this in your `StartUp.cs`:

```csharp
using Aiursoft.GptClient;

services.AddScannedDependencies();
```

That's all! All your dependencies are registered. Just use it like previous before:

```csharp
public class MyController : Controller
{
    private readonly MyScopedService _service;
    public MyController(MyScopedService service)
    {
        _service = service;
    }
}
```

### Advanced usage

When you want to register a dependency that implements an abstract, your previous way is:

```csharp
public class MyClass : IAbstract
{

}
```

```csharp
service.AddScoped<IAbstract, MyClass>();
```

That's fine. But now we want to register this automatically.

Add the dependency interface to your service like this:

```csharp
public class MyClass : IAbstract, IScopedDependency
{

}
```

When you are registering all dependencies in your `StartUp.cs`, tell us that your project supports your abstract.

```csharp
services.AddScannedDependencies(typeof(IAbstract));
```

And you can call it with multiple abstracts:

```csharp
services.AddScannedDependencies(typeof(IAbstract1), typeof(IAbstract2), typeof(IAbstract3));
```

That's all! Enjoy!
