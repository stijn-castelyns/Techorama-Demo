using Azure.AI.OpenAI;
using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.AspNetCore.Components;
using BlazorChat.Plugins;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Azure;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Microsoft.SemanticKernel.Plugins.OpenApi.Extensions;
using System.Reflection;
using U2UCourseKernel;

namespace BlazorChat.Services;

public class ChatService
{
  private IConfiguration _configuration;
  private Kernel _kernel;
  private CoursePluginKernel _coursePluginKernel;
  public static ChatHistory ChatHistory { get; private set; } = [];
  public ChatService(IConfiguration configuration, CoursePluginKernel coursePluginKernel, NavigationManager navigationManager)
  {
    _configuration = configuration;
    _coursePluginKernel = coursePluginKernel;
    _kernel = _coursePluginKernel.GetKernel();
    //_kernel.Plugins.AddFromType<PageNavigationPlugin>();
    
    ChatHistory.AddSystemMessage("You are a helpfull AI assistant. Make sure to output URLs in markdown format");
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
