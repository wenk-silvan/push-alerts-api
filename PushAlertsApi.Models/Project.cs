namespace PushAlertsApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Task>? Tasks { get; set; }
        public List<User>? Users { get; set; }
    }
}