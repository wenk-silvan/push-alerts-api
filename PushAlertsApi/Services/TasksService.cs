using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Controllers;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    public class TasksService
    {
        private readonly ILogger<TasksService> _logger;

        private readonly DbSet<Task> _dbSet;

        public TasksService(DbSet<Task> context)
        {
            _logger = new LoggerFactory().CreateLogger<TasksService>();
            _dbSet = context;
        }

        public async Task<ICollection<TaskDto>> GetTasks(Project project)
        {
            var tasks = await _dbSet.Where(t => t.ProjectId == project.Id).ToListAsync();
            _logger.LogInformation(
                $"Fetched {tasks.Count} tasks from project with uuid '{project.Uuid}' from database.");
            return TaskDto.CopyAll(project.Tasks);
        }

        public async Task<Models.Task> AddTask(Project project, TaskDto task)
        {
            var tasks = new Task(
                task.Title,
                task.Description,
                task.Source,
                task.Payload,
                project.Id
            );
            var result = await _dbSet.AddAsync(tasks);
            return result.Entity;
        }

        public void AssignTask(string uuid, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var dbTask = _dbSet.First(t => t.Uuid == Guid.Parse(uuid));
            dbTask.AssignedAt = DateTime.Now;
            dbTask.Status = TaskState.Assigned;
            dbTask.User = user;
            _dbSet.Update(dbTask);
        }
    }
}