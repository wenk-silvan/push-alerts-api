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

        public ICollection<Project> GetAllProjects(Guid uuidUser)
        {
            var projects = _dbSet
                .Where(p => p.Users.AsEnumerable().Any(u => u.Uuid == uuidUser))
                .ToList();
            _logger.LogInformation($"Fetched {projects.Count} projects from database.");
            return projects;
        }

        public Project GetProject(Guid uuid)
        {
            var project = _dbSet.Single(p => p.Uuid == uuid);
            _logger.LogInformation($"Fetched project from database with uuid: '{uuid}'.");
            return project;
        }

        public Project GetProject(int id)
        {
            var project = _dbSet.Single(p => p.Id == id);
            _logger.LogInformation($"Fetched project from database with id: '{id}'.");
            return project;
        }
    }
}