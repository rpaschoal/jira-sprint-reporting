namespace JiraSprintReporting.Atlassian;

public interface IJiraConnector
{
    Task ListSprintsAsync(string boardId, CancellationToken cancellationToken = default);

    Task GetSprintAsync(string boardId, string sprintId, CancellationToken cancellationToken = default);

    Task GetVelocityAsync(string boardId, int totalSprints, CancellationToken cancellationToken = default);
}