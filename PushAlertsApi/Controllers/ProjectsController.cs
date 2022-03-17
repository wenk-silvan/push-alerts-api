using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Data;
using PushAlertsApi.Models.Dto;
using PushAlertsApi.Services;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        private readonly DataContext _context;

        private readonly ProjectsService _projectsService;

        private readonly TasksService _tasksService;

        public ProjectsController(ILogger<ProjectsController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            _projectsService = new ProjectsService(logger, context.Projects);
            _tasksService = new TasksService(logger, context.Tasks);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Get()
        {
            try
            {
                return Ok(await _projectsService.GetAllProjects());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: No projects could be loaded.");
            }
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> Get(string uuid)
        {
            try
            {
                var project = await _projectsService.GetProject(uuid);
                var result = await _tasksService.GetTasks(project);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: No tasks could be loaded for uuid: '{uuid}'.");
            }
        }

        [HttpPost("{uuid}")]
        public async Task<ActionResult<TaskDto>> Post(string uuid, TaskDto task)
        {
            try
            {
                var newTask = await _projectsService.AddTask(uuid, task);
                await _context.SaveChangesAsync();
                return Ok(newTask);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: Task could not be add : '{uuid}'.");
            }
            catch (FormatException ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: No tasks could be loaded for uuid: '{uuid}'.");
            }
        }
    }
}