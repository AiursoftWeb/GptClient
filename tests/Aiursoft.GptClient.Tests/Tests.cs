using Aiursoft.GptClient.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiursoft.GptClient.Tests;

[TestClass]
public class Tests
{
    [TestMethod]
    public void Test1()
    {
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void TestDeserialization()
    {
        var json = """
        {
        	"model": "qwen3:32b",
        	"messages": [
        		{
        			"role": "user",
        			"content": "\u5317\u4eac\u65f6\u95f4\u73b0\u5728\u662f\u51e0\u70b9\uff1f"
        		}
        	],
        	"stream": true,
        	"tools": [
        		{
        			"type": "function",
        			"function": {
        				"type": "function",
        				"name": "tool_get_current_time_post",
        				"description": "Get current time in a specific timezones",
        				"parameters": {
        					"type": "object",
        					"properties": {
        						"timezone": {
        							"type": "string",
        							"title": "Timezone",
        							"description": "IANA timezone name (e.g., 'America/New_York', 'Europe/London'). Use 'America/New_York' as local timezone if no timezone provided by the user."
        						}
        					},
        					"required": [
        						"timezone"
        					]
        				}
        			}
        		},
        		{
        			"type": "function",
        			"function": {
        				"type": "function",
        				"name": "tool_convert_time_post",
        				"description": "Convert time between timezones",
        				"parameters": {
        					"type": "object",
        					"properties": {
        						"source_timezone": {
        							"type": "string",
        							"title": "Source Timezone",
        							"description": "Source IANA timezone name (e.g., 'America/New_York', 'Europe/London'). Use 'America/New_York' as local timezone if no source timezone provided by the user."
        						},
        						"time": {
        							"type": "string",
        							"title": "Time",
        							"description": "Time to convert in 24-hour format (HH:MM)"
        						},
        						"target_timezone": {
        							"type": "string",
        							"title": "Target Timezone",
        							"description": "Target IANA timezone name (e.g., 'Asia/Tokyo', 'America/San_Francisco'). Use 'America/New_York' as local timezone if no target timezone provided by the user."
        						}
        					},
        					"required": [
        						"time",
        						"target_timezone",
        						"source_timezone"
        					]
        				}
        			}
        		}
        	]
        }
        """;

        var model = System.Text.Json.JsonSerializer.Deserialize<OpenAiModel>(json);
        Assert.IsNotNull(model);
    }

    [TestMethod]
    public void TestSerialization()
    {
        var model = new OpenAiModel
        {
            Model = "qwen3:32b",
            Messages =
            [
                new MessagesItem()
                {
                    Role = "user",
                    Content = "北京时间现在是几点？"
                }
            ],
            Stream = true,
            Tools =
            [
                new ToolsItem()
                {
                    Type = "function",
                    Function = new FunctionDefinition
                    {
                        Type = "function",
                        Name = "tool_get_current_time_post",
                        Description = "Get current time in a specific timezones",
                        Parameters = new ParametersDefinition
                        {
                            Type = "object",
                            Properties =
                            {
                                ["timezone"] = new ParameterProperty
                                {
                                    Type = "string",
                                    Title = "Timezone",
                                    Description =
                                        "IANA timezone name (e.g., 'America/New_York', 'Europe/London'). Use 'America/New_York' as local timezone if no timezone provided by the user."
                                }
                            },
                            Required = ["timezone"]
                        }
                    }
                },

                new ToolsItem()
                {
                    Type = "function",
                    Function = new FunctionDefinition
                    {
                        Type = "function",
                        Name = "tool_convert_time_post",
                        Description = "Convert time between timezones",
                        Parameters = new ParametersDefinition
                        {
                            Type = "object",
                            Properties =
                            {
                                ["source_timezone"] = new ParameterProperty
                                {
                                    Type = "string",
                                    Title = "Source Timezone",
                                    Description =
                                        "Source IANA timezone name (e.g., 'America/New_York', 'Europe/London'). Use 'America/New_York' as local timezone if no source timezone provided by the user."
                                },
                                ["time"] = new ParameterProperty
                                {
                                    Type = "string",
                                    Title = "Time",
                                    Description = "Time to convert in 24-hour format (HH:MM)"
                                },
                                ["target_timezone"] = new ParameterProperty
                                {
                                    Type = "string",
                                    Title = "Target Timezone",
                                    Description =
                                        "Target IANA timezone name (e.g., 'Asia/Tokyo', 'America/San_Francisco'). Use 'America/New_York' as local timezone if no target timezone provided by the user."
                                }
                            },
                            Required = ["time", "target_timezone", "source_timezone"]
                        }
                    }
                }
            ]
        };
        var json = System.Text.Json.JsonSerializer.Serialize(model, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });
        Assert.IsNotNull(json);
    }
}
