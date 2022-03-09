using PushAlertsApi.Models;

namespace PushAlertsApi.Persistency.Dto
{
    public class TaskDbo
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
        public int? UserId { get; set; }
        public int Status { get; set; }
        public int ProjectId { get; set; }
    }
}