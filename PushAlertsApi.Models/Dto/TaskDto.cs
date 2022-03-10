namespace PushAlertsApi.Models.Dto
{
    public class TaskDto
    {
        public Guid Uuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string? Payload { get; set; }
        public UserDto? User { get; set; }
        public TaskState Status { get; set; }
    }
}