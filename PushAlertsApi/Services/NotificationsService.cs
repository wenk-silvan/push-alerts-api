﻿using System.Timers;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using PushAlertsApi.Util;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    public class NotificationsService
    {
        private readonly ILogger<NotificationsService> _logger;

        private readonly DbSet<TaskNotification> _dbSet;

        private readonly FirebaseMessaging _messaging;

        public NotificationsService(DbSet<TaskNotification> context, FirebaseMessaging messaging)
        {
            _logger = new LoggerFactory().CreateLogger<NotificationsService>();
            _dbSet = context;
            _messaging = messaging;

        }

        public async System.Threading.Tasks.Task NotifyUsers(string message, Project project, Task task)
        {
            var notification = new TaskNotification(message, task);
            await SendNotification(notification, project.Name);
        }

        public TaskNotification StoreNotification(TaskNotification notification)
        {
            var result = _dbSet.Add(notification);
            _logger.LogDebug($"Added new notification to database with uuid: '{notification.Uuid}'");
            return result.Entity;
        }

        private async Task<bool> SendNotification(TaskNotification notification, string topic)
        {
            try
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

                var result = await _messaging.SendAsync(message);
                _logger.LogDebug($"Sent notification to FCM with uuid: '{notification.Uuid}'");
                return true;
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogError(MessageFormatter.LogError(ex));
                return false;
            }
        }
    }
}