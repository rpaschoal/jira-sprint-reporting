using JiraSprintReporting.Atlassian.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;

namespace JiraSprintReporting.Atlassian;

public class JiraConnector : IJiraConnector
{
    private readonly RestClient _restClient;

    public JiraConnector(string jiraInstanceURL, string jiraUsername, string jiraApiToken)
    {
        _restClient = new RestClient(jiraInstanceURL, options =>
        {
            options.Authenticator = new HttpBasicAuthenticator(jiraUsername, jiraApiToken);
        });
    }

    public async Task GetSprintAsync(string boardId, string sprintId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest($"rest/greenhopper/1.0/rapid/charts/sprintreport?rapidViewId={boardId}&sprintId={sprintId}", Method.Get);
        var response = await _restClient.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var content = response.Content;
            // Parse and handle the sprint report data as needed
            Console.WriteLine(SerializeResponse(content!));
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
    }

    public async Task GetVelocityAsync(string boardId, int totalSprints, CancellationToken cancellationToken = default)
    {
        // Create a RestRequest to fetch sprint data
        var request = new RestRequest($"/rest/greenhopper/1.0/rapid/charts/velocity?rapidViewId={boardId}", Method.Get);

        // Set the query parameters to fetch data based on these
        request.AddQueryParameter("state", "closed");
        request.AddQueryParameter("maxResults", totalSprints);

        // Execute the request and get the response
        var response = await _restClient.ExecuteAsync(request);

        // Parse the response and extract the sprint data
        if (response.IsSuccessful && response.Content != null)
        {
            var deserializedResponse = DeserializeResponse<VelocityApiResponse>(response.Content);

            Console.WriteLine(SerializeResponse(response.Content));

            // TODO: Sorting by name is not ideal, might need to fetch sprints to merge and sort with date
            var sortedSprintsByTitle = deserializedResponse.Sprints.OrderByDescending(s => s.Name);

            foreach (var sprint in sortedSprintsByTitle)
            {
                var sprintId = sprint.Id.ToString();

                var velocityEntry = deserializedResponse.VelocityStatEntries[sprintId];

                var estimatedPoints = velocityEntry.Estimated.Value;
                var completedPoints = velocityEntry.Completed.Value;

                Console.WriteLine($"Sprint ID: {sprintId}, Estimated: {estimatedPoints}, Completed: {completedPoints}");
            }
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
    }

    public async Task ListSprintsAsync(string boardId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest($"rest/agile/1.0/board/{boardId}/sprint", Method.Get);
        var response = await _restClient.ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
        {
            var deserializedResponse = DeserializeResponse<ListSprintApiResponse>(response.Content);

            var content = response.Content;
            // Parse and handle the sprint report data as needed
            Console.WriteLine(SerializeResponse(content!));
            Console.WriteLine(JsonSerializer.Serialize(deserializedResponse));
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
    }

    private T DeserializeResponse<T>(string response)
    {
        return JsonSerializer.Deserialize<T>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private string SerializeResponse(string response)
    {
        // Parse and prettify the JSON output
        var jsonDocument = JsonDocument.Parse(response);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        return JsonSerializer.Serialize(jsonDocument.RootElement, options);
    }
}