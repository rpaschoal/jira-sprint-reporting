namespace JiraSprintReporting.Atlassian;

public interface IJiraConnector
{
    Task GetSprintAsync(string rapidViewId, string sprintId, CancellationToken cancellationToken = default);
}