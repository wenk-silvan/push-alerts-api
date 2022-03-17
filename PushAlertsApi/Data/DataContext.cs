using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}