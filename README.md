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

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
