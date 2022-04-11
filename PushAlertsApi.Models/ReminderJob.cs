using System.ComponentModel.DataAnnotations.Schema;

namespace PushAlertsApi.Models
{
    public class ReminderJob
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMappedAttribute] public int TaskIdForEagerLoading { get; set; }
        public virtual Task Task { get; set; }

        public ReminderJob()
        {
            Uuid = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        public ReminderJob(Task task)
        {
            Uuid = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Task = task;
            TaskIdForEagerLoading = task.Id;
        }
    }
}