namespace JiraSprintReporting.Atlassian.Models
{
    public class ListSprintApiResponse
    {
        public int MaxResults { get; set; }
        public int StartAt { get; set; }
        public bool IsLast { get; set; }
        public List<Sprint> Values { get; set; }

        public class Sprint
        {
            public int Id { get; set; }
            public string Self { get; set; }
            public string State { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime CompleteDate { get; set; }
            public int OriginBoardId { get; set; }
            public string Goal { get; set; }
        }
    }
}