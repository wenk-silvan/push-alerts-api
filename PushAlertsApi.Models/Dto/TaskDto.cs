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
        public string? UserEmail { get; set; }
        public TaskState Status { get; set; }

        public TaskDto()
        {
            Uuid = Guid.NewGuid();
            Title = string.Empty;
            Description = string.Empty;
            Source = string.Empty;
            CreatedAt = DateTime.Now;
            Status = TaskState.Opened;
        }

        public TaskDto(Task task)
        {
            Description = task.Description;
            Uuid = task.Uuid;
            Title = task.Title;
            UserEmail = task.User?.Email;
            CreatedAt = task.CreatedAt;
            AssignedAt = task.AssignedAt;
            ClosedAt = task.ClosedAt;
            Payload = task.Payload;
            Source = task.Source;
            Status = task.Status;
        }

        public static ICollection<TaskDto> CopyAll(ICollection<Task> dbTasks)
        {
            return dbTasks == null
                ? new List<TaskDto>()
                : dbTasks.Select(t => new TaskDto(t)).ToList();
        }
    }
}