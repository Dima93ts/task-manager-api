namespace TaskApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // <-- NUOVO
        public string ClientName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "todo";
        public string Priority { get; set; } = "medium";
        public DateTime DueDate { get; set; }
        public int EstimatedHours { get; set; }
    }
}
