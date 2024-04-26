namespace BlazorChat.Models;

public class Course
{
  public string CourseTitleSlug { get; set; }
  public string CourseTitle { get; set; }
  public int NumberOfDays { get; set; }

  private string learningGoals;
  public string LearningGoals
  {
    get { return learningGoals; }
    set 
    { 
      learningGoals = value;
      UpdateRecommendationInfo();
    }
  }

  private string targetAudience;

  public string TargetAudience
  {
    get { return targetAudience; }
    set 
    { 
      targetAudience = value;
      UpdateRecommendationInfo();
    }
  }

  public string RecommendationInfo { get; private set; }
  public string HtmlFileName { get; set; }

  private void UpdateRecommendationInfo()
  {
    RecommendationInfo = $"""
      TARGET_AUDIENCE:
      {TargetAudience}
      
      LEARNING_GOALS:
      {LearningGoals}
      """;
  }
}
