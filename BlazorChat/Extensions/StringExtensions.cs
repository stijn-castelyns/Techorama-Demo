using Markdig;

namespace BlazorChat.Extensions;

public static class StringExtensions
{

  public static string ToHtml(this string markdownText)
  {
    // Configure Markdig to use advanced extensions
    var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    return Markdown.ToHtml(markdownText ?? string.Empty, pipeline);
  }

}
