using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Controllers;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;

namespace PushAlertsApi.Services
{
    public class ProjectsService
    {
        private readonly ILogger<ProjectsService> _logger;

        private readonly DbSet<Project> _dbSet;

        public ProjectsService(DbSet<Project> context)
        {
            _logger = new LoggerFactory().CreateLogger<ProjectsService>();
            _dbSet = context;
        }

        public ICollection<ProjectDto> GetAllProjects()
        {
            var projects = _dbSet.ToList();
            _logger.LogInformation($"Fetched {projects.Count} projects from database.");
            return ProjectDto.CopyAll(projects);
        }

        public Project GetProject(string uuid)
        {
            var project = _dbSet.First(p => p.Uuid == Guid.Parse(uuid));
            _logger.LogInformation($"Fetched project from database with uuid: '{uuid}'.");
            return project;
        }
    }
}