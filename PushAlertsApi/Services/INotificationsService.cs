using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    /// <summary>
    /// Serves as a contract for the NotificationsService to meet the business needs.
    /// This service should do CRUD operations to the database context regarding notifications and also communicate to the push notification service.
    /// </summary>
    public interface INotificationsService
    {
        /// <summary>
        /// Notifies the users which are part of the given project. A new TaskNotification will be stored.
        /// </summary>
        /// <param name="message">The message to give to the users</param>
        /// <param name="project">The project in which users are part of</param>
        /// <param name="task">The task related to the message</param>
        /// <returns>True if sending the notification to the FCM was successful</returns>
        public bool NotifyUsers(string message, Project project, Task task);
    }
}
