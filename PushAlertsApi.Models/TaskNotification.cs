namespace PushAlertsApi.Models
{
    public class TaskNotification
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Message { get; set; }
        public virtual Task Task { get; set; }

        public TaskNotification(string message, Task task)
        {
            Uuid = Guid.NewGuid();
            Message = message;
            Task = task;
        }
    }
}