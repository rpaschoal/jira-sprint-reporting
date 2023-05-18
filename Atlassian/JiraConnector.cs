using Atlassian.Jira;

namespace JiraSprintReporting.Atlassian;

public class JiraConnector : IJiraConnector
{
    public Jira Connect(string jiraInstanceURL, string jiraUsername, string jiraApiToken)
    {
        return Jira.CreateRestClient(jiraInstanceURL, jiraUsername, jiraApiToken);
    }
}