namespace PushAlertsApi.Persistency.Dto
{
    public class ProjectDbo
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}