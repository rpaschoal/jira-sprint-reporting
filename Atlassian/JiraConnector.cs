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

    public async Task GetSprintAsync(string rapidViewId, string sprintId, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest($"rest/greenhopper/1.0/rapid/charts/sprintreport?rapidViewId={rapidViewId}&sprintId={sprintId}", Method.Get);
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