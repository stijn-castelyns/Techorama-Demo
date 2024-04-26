using Azure.AI.OpenAI;
using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.AspNetCore.Components;
using BlazorChat.Plugins;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Azure;

namespace BlazorChat.Services;

public class ChatService
{
  private IConfiguration _configuration;
  private Kernel _kernel;
  public static ChatHistory ChatHistory { get; private set; } = [];
  public ChatService(IConfiguration configuration, NavigationManager navigationManager)
  {
    _configuration = configuration;
    IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.Services.AddSingleton(navigationManager);
    kernelBuilder.Services.AddAzureClients(options =>
    {
      options.AddSearchClient(_configuration.GetSection("AzureAISearch"));
    });

    kernelBuilder.Plugins.AddFromType<PageNavigationPlugin>();
    kernelBuilder.Plugins.AddFromType<CourseRecommendationPlugin>();

    AzureKeyCredential azureKeyCredential = new(_configuration["AzureOpenAI:AzureKeyCredential"]!);
    string deploymentName = _configuration["AzureOpenAI:DeploymentName"]!;
    Uri endpoint = new Uri(_configuration["AzureOpenAI:Endpoint"]!);

    OpenAIClient openAIClient = new(endpoint, azureKeyCredential);

    kernelBuilder.AddAzureOpenAIChatCompletion(deploymentName, openAIClient);
    _kernel = kernelBuilder.Build();
    ChatHistory.AddSystemMessage("You are a helpfull AI assistant. Make sure to output URLs in markdown format");
  }

  public async Task<string> SendMessageAsync(string message)
  {
    OpenAIPromptExecutionSettings executionSettings = new OpenAIPromptExecutionSettings()
    {
      ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };
    // Retrieve the chat completion service from the kernel
    IChatCompletionService chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

    ChatHistory.AddUserMessage(message);
    var result = await chatCompletionService.GetChatMessageContentsAsync(ChatHistory, kernel: _kernel, executionSettings: executionSettings);

    string assistantMessage = result[0].ToString();
    ChatHistory.AddAssistantMessage(assistantMessage);
    return assistantMessage;
  }

  public async IAsyncEnumerable<string> SendMessageStreamAsync(string message)
  {
    // Retrieve the chat completion service from the kernel
    OpenAIPromptExecutionSettings executionSettings = new OpenAIPromptExecutionSettings()
    {
      ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };
    IChatCompletionService chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

    ChatHistory.AddUserMessage(message);
    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(ChatHistory, kernel: _kernel, executionSettings: executionSettings);
    string fullMessage = string.Empty;
    await foreach(var chunk in result)
    {
      fullMessage += chunk.Content;
      yield return chunk.Content;
    }
    ChatHistory.AddAssistantMessage(fullMessage);
  }
}
