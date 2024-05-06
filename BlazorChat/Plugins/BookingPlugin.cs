using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using U2U_AI.API.Requests;
using U2U_AI.Infra.Entities;

namespace BlazorChat.Plugins;

public class BookingPlugin
{
  [KernelFunction]
  [Description("Requests the available dates that a U2U course takes place.")]
  public async Task<string> RequestBookingDatesAsync(
        Kernel kernel,
        [Description("The slugified name of a U2U course. Some examples include 'building-aspnet-web-apis', building-web-applications-with-aspnet, 'developing-ai-powered-apps-with-azure-ai-services'")] string courseNameSlug
    )
  {
    using (HttpClient httpClient = new HttpClient())
    {
      httpClient.BaseAddress = new Uri("https://localhost:7250/");
      httpClient.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));

      var response = await httpClient.GetAsync($"api/sessions/{courseNameSlug}");
      if (response.IsSuccessStatusCode)
      {
        var jsonResponse = await response.Content.ReadAsStreamAsync();
        var sessions = await JsonSerializer.DeserializeAsync<List<Session>>(jsonResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        string message = $"The available dates for course {courseNameSlug} are";
        foreach (var session in sessions)
        {
          message += $"\n \t + {session.OrganisationDays.First()}";
        }
        message += "\nPlease select one of these dates, along with your first and last name to make a booking.";
        return message;
      }
    }
    // Return the message back to the agent
    return "Unable to find dates for given request";
  }

  [KernelFunction]
  [Description("Makes a booking for a U2U course.")]
  public async Task<string> MakeBookingAsync(
        Kernel kernel,
        [Description("The slugified name of a U2U course. Some examples include 'building-aspnet-web-apis', building-web-applications-with-aspnet, 'developing-ai-powered-apps-with-azure-ai-services'")] string courseNameSlug,
        [Description("The first name")] string firstName,
        [Description("The last name")] string lastName,
        [Description("The date the course will take place on")] string dateString
    )
  {
    using (HttpClient httpClient = new HttpClient())
    {
      httpClient.BaseAddress = new Uri("https://localhost:7250");
      httpClient.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));

      var sessionsResponse = await httpClient.GetAsync($"api/sessions/{courseNameSlug}");
      var jsonResponse = await sessionsResponse.Content.ReadAsStreamAsync();
      List<Session> sessions = await JsonSerializer.DeserializeAsync<List<Session>>(jsonResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
      DateOnly date = DateOnly.Parse(dateString);
      Session session = sessions.FirstOrDefault(s => s.OrganisationDays.Any(d => d == date));

      BookingRequest booking = new ()
      {
        FirstName = firstName,
        LastName = lastName,
        SessionId = session.Id
      };

      string jsonBody = JsonSerializer.Serialize(booking, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
      var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

      var response = await httpClient.PostAsync($"api/bookings", body);
      if (response.IsSuccessStatusCode)
      {
        return "Your booking has been processed succesfully!";
      }
    }
    // Return the message back to the agent
    return "Something went wrong!";
  }
}
