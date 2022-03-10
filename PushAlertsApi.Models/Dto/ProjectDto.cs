namespace PushAlertsApi.Models.Dto
{
    public class ProjectDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TaskDto> Tasks { get; set; }
    }
}