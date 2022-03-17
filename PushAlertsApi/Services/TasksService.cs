using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Controllers;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    public class TasksService
    {
        private readonly ILogger<ProjectsController> _logger;

        private readonly DbSet<Task> _dbSet;

        public TasksService(ILogger<ProjectsController> logger, DbSet<Task> context)
        {
            _logger = logger;
            _dbSet = context;
        }
        public async Task<ICollection<TaskDto>> GetTasks(Project project)
        {
            var tasks = await _dbSet.Where(t => t.ProjectId == project.Id).ToListAsync();
            _logger.LogInformation($"Fetched {tasks.Count} tasks from project with uuid '{project.Uuid}' from database.");
            return TaskDto.CopyAll(project.Tasks);
        }
    }
}
