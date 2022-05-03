using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Data
{
    /// <summary>
    /// This class represents the PushAlerts database structure.
    /// Each DbSet property represents a database table which is created or updated during an Entity Framework migration.
    /// </summary>
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<TaskNotification> Notifications { get; set; }
    }
}