using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;
using PushAlertsApi.Services;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        private readonly DataContext _context;

        private readonly ProjectsService _projectsService;

        private readonly TasksService _tasksService;

        private readonly UsersService _usersService;

        public TasksController(ILogger<ProjectsController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            _projectsService = new ProjectsService(context.Projects);
            _tasksService = new TasksService(context.Tasks);
            _usersService = new UsersService(context.Users);
        }

        [HttpGet("{uuid}")]
        public ActionResult<IEnumerable<TaskDto>> Get(string uuid)
        {
            try
            {
                var project = _projectsService.GetProject(uuid);
                var tasks = TaskDto.CopyAll(project.Tasks);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: No tasks could be loaded for uuid: '{uuid}'.");
            }
        }

        [HttpPost("{uuidProject}")]
        public ActionResult<TaskDto> Post(string uuidProject, TaskDto task)
        {
            try
            {
                var project = _projectsService.GetProject(uuidProject);
                var newTask = _tasksService.AddTask(project, task);
                _context.SaveChanges();
                return Ok(newTask);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: Task could not be add : '{uuidProject}'.");
            }
            catch (FormatException ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: No tasks could be loaded for uuidProject: '{uuidProject}'.");
            }
        }

        [HttpPut("{uuidTask}/assign/{uuidUser}")]
        public ActionResult<TaskDto> Put(string uuidTask, string uuidUser)
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
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: Task could not be assigned");
            }
        }

        [HttpPut("{uuidTask}/close/")]
        public ActionResult<TaskDto> Put(string uuidTask, TaskState status)
        {
            try
            {
                _tasksService.CloseTask(uuidTask, status);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: Task could not be assigned");
            }
        }
    }
}