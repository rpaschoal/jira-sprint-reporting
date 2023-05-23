namespace JiraSprintReporting.Atlassian.Models
{
    public sealed class VelocityApiResponse
    {
        public List<Sprint> Sprints { get; set; }
        public Dictionary<string, VelocityStatEntry> VelocityStatEntries { get; set; }

        public sealed class VelocityStatEntry
        {
            public ValueTextEntry Estimated { get; set; }
            public ValueTextEntry Completed { get; set; }
            public List<string> AllConsideredIssueKeys { get; set; }
            public List<IssueKeyValue> EstimatedEntries { get; set; }
            public List<IssueKeyValue> CompletedEntries { get; set; }
        }

        public sealed class ValueTextEntry
        {
            public double Value { get; set; }
            public string Text { get; set; }
        }

        public sealed class IssueKeyValue
        {
            public string IssueKey { get; set; }
            public double? Value { get; set; }
        }

        public sealed class Sprint
        {
            public int Id { get; set; }
            public int Sequence { get; set; }
            public string Name { get; set; }
            public string State { get; set; }
            public int LinkedPagesCount { get; set; }
            public string Goal { get; set; }
            public int SprintVersion { get; set; }
        }
    }
}