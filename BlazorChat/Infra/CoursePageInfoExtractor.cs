using BlazorChat.ViewModels;
using HtmlAgilityPack;
using System.Text;

namespace BlazorChat.Infra;

public static class CoursePageInfoExtractor
{
  public static U2UCourseHtml ExtractCourseInfo(string htmlContent)
  {
    var doc = new HtmlDocument();
    doc.LoadHtml(htmlContent);

    var course = new U2UCourseHtml();

    // Extracting the title
    var titleNode = doc.DocumentNode.SelectSingleNode("//head/title");
    course.CourseTitle = titleNode?.InnerText ?? string.Empty;

    // Extracting the number of days
    var metaNode = doc.DocumentNode.SelectSingleNode("//meta[@name='NumberOfDays']");
    if (metaNode != null && int.TryParse(metaNode.GetAttributeValue("content", "0"), out int numberOfDays))
    {
      course.NumberOfDays = numberOfDays;
    }

    // Extracting Learning Goals
    var learningGoalsNode = doc.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Learning Goals')]/following-sibling::p");
    course.LearningGoals = learningGoalsNode?.InnerText.Trim() ?? string.Empty;

    // Extracting Target Audience
    var targetAudienceNode = doc.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Target Audience')]/following-sibling::p");
    course.TargetAudience = targetAudienceNode?.InnerText.Trim() ?? string.Empty;

    // Extracting Course Outline
    var outlineNode = doc.DocumentNode.SelectSingleNode("//h2[contains(text(), 'Course Outline')]");
    if (outlineNode != null)
    {
      var sb = new StringBuilder();
      var currentNode = outlineNode.NextSibling;
      while (currentNode != null)
      {
        if (currentNode.NodeType == HtmlNodeType.Element)
        {
          sb.AppendLine(currentNode.OuterHtml.Trim());
        }
        currentNode = currentNode.NextSibling;
      }
      course.CourseOutline = sb.ToString().Trim();
    }

    return course;
  }
}
