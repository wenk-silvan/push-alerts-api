using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PushAlertsApi.Models
{
    public class Task
    {
        [JsonIgnore] public int Id { get; set; }

        [JsonIgnore] public int ProjectId { get; set; }

        public Guid Uuid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Source { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? AssignedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public string? Payload { get; set; }

        [NotMapped] public string? UserEmail { get; set; }

        [JsonIgnore] public virtual User? User { get; set; }

        public TaskState Status { get; set; }

        public Task(string title, string description, string source, int projectId, string? payload)
        {
            Uuid = Guid.NewGuid();
            Title = title;
            Description = description;
            Source = source;
            CreatedAt = DateTime.Now;
            ProjectId = projectId;
            Payload = payload;
            Status = TaskState.Opened;
        }
    }
}