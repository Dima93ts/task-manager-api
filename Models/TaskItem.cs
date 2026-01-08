namespace TaskApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // "todo" | "in_progress" | "done"
        public string Status { get; set; } = "todo";
        // "low" | "medium" | "high"
        public string Priority { get; set; } = "medium";
        public DateTime DueDate { get; set; }
        public int EstimatedHours { get; set; }
    }
}
