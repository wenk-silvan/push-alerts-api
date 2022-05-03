using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PushAlertsApi.Data;
using PushAlertsApi.Filters;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using PushAlertsApi.Util;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Controllers
{
    /// <summary>
    /// This Api Controller is responsible for all tasks related requests.
    /// It is authorized and the endpoints can only be reached if the user is authenticated towards the server.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        private readonly DataContext _context;

        private readonly IProjectsService _projectsService;

        private readonly ITasksService _tasksService;

        private readonly IUsersService _usersService;

        private readonly INotificationsService _notificationsService;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IConfiguration _configuration;

        public TasksController(IConfiguration configuration, ILogger<ProjectsController> logger, DataContext context,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _projectsService = new ProjectsService(context.Projects);
            _tasksService = new TasksService(context.Tasks);
            _usersService = new UsersService(context.Users);
            _notificationsService = new NotificationsService(context.Notifications, FirebaseMessaging.DefaultInstance);
            _context.SaveChanges();
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Calls the ProjectsService instance to get all tasks of the given project uuid using EF lazy-loading
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns>A list of tasks which are part of the project with the given uuid</returns>
        [HttpGet("{uuid}")]
        public ActionResult<IEnumerable<Task>> GetAllOfProject(string uuid)
        {
            try
            {
                var project = _projectsService.GetProject(uuid);
                var tasks = project.Tasks;
                tasks?.ForEach(t => t.UserEmail = t.User?.Email);
                return Ok(project.Tasks);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        /// <summary>
        /// Calls the ProjectsService instance to fetch the project with the given uuid and the TasksService instance to add the given task to this project.
        /// Then notifies users via the topic through the NotificationsService instance and registers a timer to setup the reminder job.
        /// This endpoint allows anonymous access but requires the ApiKey (see ApiKey Attribute).
        /// </summary>
        /// <param name="key">The api key as configured in the appsettings.json</param>
        /// <param name="uuidProject">The uuid of the project that the task should be added</param>
        /// <param name="task">The task with its required and optional properties</param>
        /// <returns></returns>
        [ApiKeyAuth]
        [HttpPost("{uuidProject}"), AllowAnonymous]
        public async Task<ActionResult<Task>> Add([FromHeader(Name = "ApiKey")] string key, string uuidProject,
            NewTask task)
        {
            try
            {
                var project = _projectsService.GetProject(uuidProject);
                var newTask = _tasksService.AddTask(project, task);
                await _context.SaveChangesAsync();
                _notificationsService.NotifyUsers($"Project {project.Name} has new task from {task.Source}", project,
                    newTask);
                var seconds = int.TryParse(_configuration.GetSection("ReminderSeconds").Value, out var result)
                    ? result
                    : 1800;
                var timer = new System.Timers.Timer(seconds * 1000);
                timer.Elapsed += ((sender, args) => OnReminderJobTimerFinish(timer, newTask.Uuid));
                timer.Enabled = true;
                await _context.SaveChangesAsync();
                return Ok(newTask);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        /// <summary>
        /// Calls the usersService instance to fetch the user with the give uuid and the TasksService instance to assign this user to the task.
        /// </summary>
        /// <param name="uuidTask">The uuid of the task that is modified</param>
        /// <param name="uuidUser">The uuid of the user that should be assigned</param>
        /// <returns>Empty successful http response</returns>
        [HttpPut("{uuidTask}/assign/{uuidUser}")]
        public ActionResult Assign(string uuidTask, string uuidUser)
        {
            try
            {
                var user = _usersService.GetUserByUuid(uuidUser);
                _tasksService.AssignTask(uuidTask, user);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        /// <summary>
        /// Calls the TasksService instance to close the task
        /// </summary>
        /// <param name="uuidTask">The uuid of the task that is modified</param>
        /// <param name="status">The status to which the task should be set</param>
        /// <returns></returns>
        [HttpPut("{uuidTask}/close/")]
        public ActionResult Close(string uuidTask, TaskState status)
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

        private void OnReminderJobTimerFinish(IDisposable timer, Guid taskUuid)
        {
            // Create new database context and tasks service because this gets executed in a separate thread.
            using var scope = _serviceScopeFactory.CreateScope();
            using var currentDbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var task = new TasksService(currentDbContext.Tasks).GetTask(taskUuid.ToString());
            var project = new ProjectsService(currentDbContext.Projects).GetProject(task.ProjectId);
            if (task.Status == TaskState.Opened)
            {
                new NotificationsService(currentDbContext.Notifications, FirebaseMessaging.DefaultInstance).NotifyUsers(
                    $"Reminder for task '{task.Title}' in project {project.Name}", project, task);
            }
            else
            {
                timer.Dispose();
            }

            currentDbContext.SaveChanges();
        }
    }
}