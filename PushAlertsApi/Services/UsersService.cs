using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    public class UsersService
    {
        private readonly ILogger<UsersService> _logger;

        private readonly DbSet<User> _dbSet;

        public UsersService(DbSet<User> context)
        {
            _logger = new LoggerFactory().CreateLogger<UsersService>();
            _dbSet = context;
        }

        public User GetUser(string uuid)
        {
            var user = _dbSet.First(u => u.Uuid == Guid.Parse(uuid));
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "No user found.");
            }
            _logger.LogInformation($"Fetched user from database with uuid: '{uuid}'.");
            return user;
        }
    }
}