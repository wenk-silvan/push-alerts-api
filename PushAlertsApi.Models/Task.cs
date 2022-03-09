namespace PushAlertsApi.Models
{
    public class Task
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string? Payload { get; set; }
        public User? User { get; set; }
        public TaskState Status { get; set; }
    }
}