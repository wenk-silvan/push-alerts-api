using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;

namespace PushAlertsApi.Services
{
    /// <summary>
    /// See interface for code documentation
    /// </summary>
    public class ProjectsService : IProjectsService
    {
        private readonly ILogger<ProjectsService> _logger;

        private readonly DbSet<Project> _dbSet;

        public ProjectsService(DbSet<Project> context)
        {
            _logger = new LoggerFactory().CreateLogger<ProjectsService>();
            _dbSet = context;
        }

        public ICollection<Project> GetAllProjects(string uuidUser)
        {
            var uuid = Guid.TryParse(uuidUser, out var result) ? result : Guid.Empty;
            var projects = _dbSet
                .Where(p => p.Users.ToList().Any(u => u.Uuid == uuid))
                .ToList();
            _logger.LogInformation($"Fetched {projects.Count} projects from database.");
            return projects;
        }

        public Project GetProject(string uuid)
        {
            var project = _dbSet.Single(p => p.Uuid == Guid.Parse(uuid));
            _logger.LogInformation($"Fetched project from database with uuid: '{uuid}'.");
            return project;
        }

        public Project GetProject(int id)
        {
            var project = _dbSet.First(p => p.Id == id);
            _logger.LogInformation($"Fetched project from database with id: '{id}'.");
            return project;
        }
    }
}