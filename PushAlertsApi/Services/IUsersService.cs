
using PushAlertsApi.Models;

namespace PushAlertsApi.Services
{
    public interface IUsersService
    {
        /// <summary>
        /// Returns the user for the given UUID
        /// </summary>
        /// <param name="uuid">A unique identifier for a user</param>
        /// <returns></returns>
        public User GetUser(string uuid);
    }
}
