using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;

namespace PushAlertsApi.Persistency
{
    public class PushAlertsDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}