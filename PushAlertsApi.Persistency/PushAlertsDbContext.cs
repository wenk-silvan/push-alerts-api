using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Persistency.Dto;

namespace PushAlertsApi.Persistency
{
    public class PushAlertsDbContext : DbContext
    {
        public DbSet<ProjectDbo> Projects { get; set; }
        public DbSet<TaskDbo> Tasks { get; set; }
        public DbSet<UserDbo> Users { get; set; }
    }
}