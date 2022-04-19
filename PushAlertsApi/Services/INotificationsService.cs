using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    public interface INotificationsService
    {
        /// <summary>
        /// Notifies the users which are part of the given project. A new TaskNotification will be stored.
        /// </summary>
        /// <param name="message">The message to give to the users</param>
        /// <param name="project">The project in which users are part of</param>
        /// <param name="task">The task related to the message</param>
        /// <returns></returns>
        public bool NotifyUsers(string message, Project project, Task task);
    }
}
