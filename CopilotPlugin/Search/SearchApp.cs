using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using AdaptiveCards;
using Newtonsoft.Json.Linq;
using U2UCourseKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Azure.Core;
using U2U_AI.Infra.Entities;

namespace CopilotPlugin.Search;

public class SearchApp : TeamsActivityHandler
{
  private readonly string _adaptiveCardFilePath = Path.Combine(".", "Resources", "helloWorldCard.json");

  private readonly IConfiguration _configuration;
  private readonly CoursePluginKernel _coursePluginKernel;
  public SearchApp(IConfiguration configuration, CoursePluginKernel coursePluginKernel)
  {
    this._configuration = configuration;
    this._coursePluginKernel = coursePluginKernel;
  }

  // Search
  protected override async Task<MessagingExtensionResponse> OnTeamsMessagingExtensionQueryAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionQuery query, CancellationToken cancellationToken)
  {
    var templateJson = await File.ReadAllTextAsync(_adaptiveCardFilePath, cancellationToken);
    var template = new AdaptiveCards.Templating.AdaptiveCardTemplate(templateJson);

    var text = query?.Parameters?[0]?.Value as string ?? string.Empty;
    List<Course> courses = await KernelPluginResponse(text);
    // We take every row of the results and wrap them in cards wrapped in in MessagingExtensionAttachment objects.
    var attachments = courses.Select(course =>
    {
      var previewCard = new ThumbnailCard { Title = course.CourseTitle };

      var adaptiveCardJson = template.Expand(new { name = course.CourseTitle, description = course.LearningGoals });
      var adaptiveCard = AdaptiveCard.FromJson(adaptiveCardJson).Card;
      
      var attachment = new MessagingExtensionAttachment
      {
        ContentType = AdaptiveCard.ContentType,
        Content = adaptiveCard,
        Preview = previewCard.ToAttachment()
      };

      return attachment;
    }).ToList();

    return new MessagingExtensionResponse
    {
      ComposeExtension = new MessagingExtensionResult
      {
        Type = "result",
        AttachmentLayout = "list",
        Attachments = attachments
      }
    };
  }

  // Generate a set of substrings to illustrate the idea of a set of results coming back from a query. 
  private async Task<List<Course>> KernelPluginResponse(string text)
  {
    Kernel kernel = _coursePluginKernel.GetKernel();
    List<Course> courses = await kernel.InvokeAsync<List<Course>>(pluginName: "CourseRecommendationPlugin",
                       functionName: "GetRelevantCourses",
                       arguments: new() { { "userQuery", text } });

    return courses;
  }
}