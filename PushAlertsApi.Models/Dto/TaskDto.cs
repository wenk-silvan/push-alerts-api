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
        public string? UserEmail{ get; set; }
        public TaskState Status { get; set; }

        public static ICollection<TaskDto> CopyAll(ICollection<Task> dbTasks)
        {
            return dbTasks == null ? 
                new List<TaskDto>() : dbTasks.Select(Copy).ToList();
        }

        public static TaskDto Copy(Task dbTask)
        {
            return new TaskDto
            {
                Description = dbTask.Description,
                Uuid = dbTask.Uuid,
                Title = dbTask.Title,
                UserEmail = dbTask.User?.Email,
                CreatedAt = dbTask.CreatedAt,
                AssignedAt = dbTask.AssignedAt,
                ClosedAt = dbTask.ClosedAt,
                Payload = dbTask.Payload,
                Source = dbTask.Source,
                Status = dbTask.Status
            };
        }
    }
}