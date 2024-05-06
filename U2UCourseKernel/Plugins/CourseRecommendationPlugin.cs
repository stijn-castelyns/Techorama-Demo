using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using U2U_AI.Infra.Entities;

namespace U2UCourseKernel.Plugins;

public class CourseRecommendationPlugin
{
  private SearchClient _searchClient;

  public CourseRecommendationPlugin(SearchClient searchClient)
  {
    _searchClient = searchClient;
  }

  [KernelFunction, Description("Based on a user question related to which course might be best suited for them, this function return descriptions for U2U course that the user might be interested in")]
  public async Task<List<Course>> GetRelevantCourses([Description("The users question related to which course they should follow")]string userQuery)
  {
    // Create the query to send to the search service
    VectorizableTextQuery vectorQuery = new VectorizableTextQuery(userQuery)
    {
      KNearestNeighborsCount = 3,
      Fields = { "RecommendationInfoEmbedding" },
      Exhaustive = true
    };

    SearchResults<Course> searchResults = await _searchClient.SearchAsync<Course>(new SearchOptions()
    {
      VectorSearch = new VectorSearchOptions()
      {
        Queries = { vectorQuery }
      }
    });
    List<Course> courses = searchResults.GetResults().Select(s => s.Document).ToList();
    return courses;
  }

  static List<string> CreateStringRepresentation(SearchResults<Course> searchResults)
  {
    List<string> stringResults = [];
    int index = 1;
    foreach (SearchResult<Course> searchResult in searchResults.GetResults())
    {
      Course course = searchResult.Document;

      string tempResult = $"""
      -------------------- COURSE {index} --------------------
      COURSE_TITLE: {course.CourseTitle}
      COURSE_LENGTH_IN_DAYS: {course.NumberOfDays}
      RELATIVE_WEBPAGE_URL: /courses/{course.CourseTitleSlug}
      DOCUMENT_CONTENT:
      {course.RecommendationInfo}
      ----------------------------------------------------------
      """;
      stringResults.Add(tempResult);
      index++;
    }
    return stringResults;
  }
}
