using Atlassian.Jira;

namespace JiraSprintReporting.Atlassian;

public interface IJiraConnector
{
    Jira Connect(string jiraInstanceURL, string jiraUserName, string jiraApiToken);
}