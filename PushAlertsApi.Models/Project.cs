namespace PushAlertsApi.Models
{
    public class Project
    {
        public Project() {}
        public Project(Guid uuid, string name, string description, IEnumerable<Task> tasks)
        {
            Uuid = uuid;
            Name = name;
            Description = description;
            Tasks = tasks;
        }

        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}