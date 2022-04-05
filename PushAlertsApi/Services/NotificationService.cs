using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    public class NotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        private readonly DbSet<TaskNotification> _dbSet;

        public NotificationService(DbSet<TaskNotification> context)
        {
            _logger = new LoggerFactory().CreateLogger<NotificationService>();
            _dbSet = context;
        }

        public void NotifyForNewTask(Project project, Task task)
        {
            var message = $"New task from {task.Source}";
            var notification = new TaskNotification(message, task);
            SendNotification(notification, project.Name);
        }

        public void NotifyForReminder(Project project, Task task)
        {
            var message = $"Reminder for new task from {task.Source}";
            var notification = new TaskNotification(message, task);
            SendNotification(notification, project.Name);
        }

        public TaskNotification StoreNotification(TaskNotification notification)
        {
            var result = _dbSet.Add(notification);
            _logger.LogInformation($"Added new notification to database with uuid: '{notification.Uuid}'");
            return result.Entity;
        }

        private async void SendNotification(TaskNotification notification, string topic)
        {
            var message = new Message()
            {
                Data = new Dictionary<string, string>(),
                Notification = new Notification
                {
                    Title = notification.Message,
                    Body = notification.Task.Title
                },
                Topic = topic
            };

            var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}