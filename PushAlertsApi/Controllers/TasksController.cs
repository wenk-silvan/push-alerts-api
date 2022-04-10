using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;
using PushAlertsApi.Services;
using PushAlertsApi.Util;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        private readonly DataContext _context;

        private readonly ProjectsService _projectsService;

        private readonly TasksService _tasksService;

        private readonly UsersService _usersService;

        private readonly NotificationsService _notificationsService;

        private readonly ReminderJobService _reminderJobService;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private const int ReminderIntervalMillis = 10000;

        public TasksController(ILogger<ProjectsController> logger, DataContext context,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _context = context;
            _projectsService = new ProjectsService(context.Projects);
            _tasksService = new TasksService(context.Tasks);
            _usersService = new UsersService(context.Users);
            _notificationsService = new NotificationsService(context.Notifications, FirebaseMessaging.DefaultInstance);
            _reminderJobService = new ReminderJobService(context.ReminderJobs, ReminderIntervalMillis);
            _reminderJobService.CleanUp();
            _context.SaveChanges();
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet("{uuid}")]
        public ActionResult<IEnumerable<TaskDto>> Get(string uuid)
        {
            try
            {
                var project = _projectsService.GetProject(uuid);
                var tasks = TaskDto.CopyAll(project.Tasks!);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost("{uuidProject}")]
        public async Task<ActionResult<TaskDto>> Post(string uuidProject, TaskDto task)
        {
            try
            {
                var project = _projectsService.GetProject(uuidProject);
                var newTask = _tasksService.AddTask(project, task);
                _notificationsService.NotifyUsers($"Project {project.Name} has new task from {task.Source}", project,
                    newTask);

                _reminderJobService.Add(new ReminderJob(newTask), project, OnReminderJobTimerFinish);
                await _context.SaveChangesAsync();
                return Ok(newTask);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPut("{uuidTask}/assign/{uuidUser}")]
        public ActionResult Put(string uuidTask, string uuidUser)
        {
            try
            {
                var user = _usersService.GetUser(uuidUser);
                _tasksService.AssignTask(uuidTask, user);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPut("{uuidTask}/close/")]
        public ActionResult Put(string uuidTask, TaskState status)
        {
            try
            {
                _tasksService.CloseTask(uuidTask, status);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        private ActionResult HandleApiException(Exception ex)
        {
            _logger.LogError(MessageFormatter.LogError(ex));
            return BadRequest(MessageFormatter.ActionResultBadRequest());
        }

        private void OnReminderJobTimerFinish(IDisposable timer, Guid taskUuid, Project project)
        {
            // Create new database context and tasks service because this gets executed in a separate thread.
            using var scope = _serviceScopeFactory.CreateScope();
            using var currentDbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var task = new TasksService(currentDbContext.Tasks).GetTask(taskUuid.ToString());
            if (task.Status == TaskState.Opened)
            {
                new NotificationsService(currentDbContext.Notifications, FirebaseMessaging.DefaultInstance).NotifyUsers(
                    $"Reminder for task '{task.Title}' in project {project.Name}", project, task);
            }
            else
            {
                timer.Dispose();
                new ReminderJobService(currentDbContext.ReminderJobs, ReminderIntervalMillis).DeleteAll(task);
                currentDbContext.SaveChanges();
            }
        }
    }
}