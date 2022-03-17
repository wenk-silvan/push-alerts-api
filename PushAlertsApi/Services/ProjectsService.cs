using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Controllers;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;

namespace PushAlertsApi.Services
{
    public class ProjectsService
    {
        private readonly ILogger<ProjectsController> _logger;

        private readonly DbSet<Project> _dbSet;

        public ProjectsService(ILogger<ProjectsController> logger, DbSet<Project> context)
        {
            _logger = logger;
            _dbSet = context;
        }

        public async Task<ICollection<ProjectDto>> GetAllProjects()
        {
            var projects = await _dbSet.ToListAsync();
            _logger.LogInformation($"Fetched {projects.Count} projects from database.");
            return ProjectDto.CopyAll(projects);
        }

        public async Task<Project> GetProject(string uuid)
        {
            var project = await _dbSet.FirstAsync(p => p.Uuid == Guid.Parse(uuid));
            _logger.LogInformation($"Fetched project from database with uuid: '{uuid}'.");
            return project;
        }

        public async Task<TaskDto> AddTask(string uuid, TaskDto task)
        {
            var project = await _dbSet.FirstAsync(p => p.Uuid == Guid.Parse(uuid));
            var newTask = new Models.Task(
                task.Title,
                task.Description,
                task.Source,
                task.Payload
            );
            project.Tasks ??= new List<Models.Task>();
            project.Tasks.Add(newTask);
            _dbSet.Update(project);
            return TaskDto.Copy(newTask);
        }
    }
}