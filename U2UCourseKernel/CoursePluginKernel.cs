using Azure.AI.OpenAI;
using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Plugins.OpenApi.Extensions;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using System.Reflection;
using U2UCourseKernel.Plugins;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace U2UCourseKernel;

public class CoursePluginKernel
{
  private readonly IConfiguration _configuration;
  private readonly IKernelBuilder _kernelBuilder = Kernel.CreateBuilder();
  private readonly Kernel _kernel;
  
  public CoursePluginKernel(IConfiguration configuration, NavigationManager navigationManager = null)
  {
    _configuration = configuration;

    if(navigationManager is not null)
    {
      _kernelBuilder.Services.AddSingleton(navigationManager);
    }

    _kernelBuilder.Services.AddLogging((options) =>
    {
      options.SetMinimumLevel(LogLevel.Trace);
      options.AddConsole();
    });

    _kernelBuilder.Services.AddAzureClients(options =>
    {
      options.AddSearchClient(_configuration.GetSection("AzureAISearch"));
    });

    _kernelBuilder.Plugins.AddFromType<CourseRecommendationPlugin>();
    _kernelBuilder.Plugins.AddFromType<BookingPlugin>();

    AzureKeyCredential azureKeyCredential = new(_configuration["AzureOpenAI:AzureKeyCredential"]!);
    string deploymentName = _configuration["AzureOpenAI:DeploymentName"]!;
    Uri endpoint = new Uri(_configuration["AzureOpenAI:Endpoint"]!);

    OpenAIClient openAIClient = new(endpoint, azureKeyCredential);

    _kernelBuilder.AddAzureOpenAIChatCompletion(deploymentName, openAIClient);
    _kernel = _kernelBuilder.Build();

    // Load prompt from YAML
    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("U2UCourseKernel.Plugins.Prompts.CourseRecommendation.recommendCourse.prompt.yaml")!;
    using StreamReader reader = new(stream);

    KernelFunction courseRecommender = _kernel.CreateFunctionFromPromptYaml(
        reader.ReadToEnd(),
        promptTemplateFactory: new HandlebarsPromptTemplateFactory()
    );

    _kernel.Plugins.AddFromFunctions("CourseRecommender", [courseRecommender]);
  }
  public Kernel GetKernel()
  {
    return _kernel;
  }
}
