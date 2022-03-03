namespace PushAlertsApi.Models
{
    public class Task
    {
        public Task() {}

        public Task(int id, Guid uuid, string title, string description, string source, DateTime createdAt, DateTime? assignedAt, DateTime? closedAt, string? payload, User? user, TaskState status)
        {
            Id = id;
            Uuid = uuid;
            Title = title;
            Description = description;
            Source = source;
            CreatedAt = createdAt;
            AssignedAt = assignedAt;
            ClosedAt = closedAt;
            Payload = payload;
            User = user;
            Status = status;
        }

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