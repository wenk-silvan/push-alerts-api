using System.Text.Json.Serialization;

namespace PushAlertsApi.Models
{
    public class Project
    {
        [JsonIgnore] public int Id { get; set; }

        public Guid Uuid { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [JsonIgnore] public virtual List<Task>? Tasks { get; set; } = new();

        [JsonIgnore] public virtual List<User>? Users { get; set; } = new();
    }
}