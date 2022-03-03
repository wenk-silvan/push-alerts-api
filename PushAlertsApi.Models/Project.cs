namespace PushAlertsApi.Models
{
    public class Project
    {
        public Project(Guid uuid, string name, string description)
        {
            Uuid = uuid;
            Name = name;
            Description = description;
        }

        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}