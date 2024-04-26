using Markdig;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorChat.Extensions;

public static class StringExtensions
{

  public static string ToHtml(this string markdownText)
  {
    // Configure Markdig to use advanced extensions
    var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    return Markdown.ToHtml(markdownText ?? string.Empty, pipeline);
  }

  public static string ToUrlSlug(this string value)
  {

    //First to lower case
    value = value.ToLowerInvariant();

    //Remove all accents
    var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
    value = Encoding.ASCII.GetString(bytes);

    //Replace spaces
    value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

    //Remove invalid chars
    value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

    //Trim dashes from end
    value = value.Trim('-', '_');

    //Replace double occurences of - or _
    value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

    return value;
  }

}
