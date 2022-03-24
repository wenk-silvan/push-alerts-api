namespace PushAlertsApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual List<Task>? Tasks { get; set; } = new();
        public virtual List<User>? Users { get; set; } = new();
    }
}