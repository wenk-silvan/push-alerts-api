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
            _projectsService = new ProjectsService(context.Projects);
            _tasksService = new TasksService(context.Tasks);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProjectDto>> Get()
        {
            try
            {
                return Ok(_projectsService.GetAllProjects());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Message: {ex.Message}, Inner Exception Message: " +
                                 $"{ex.InnerException?.Message}, Stack Trace: {ex.StackTrace}");
                return BadRequest($"Unexpected error: No projects could be loaded.");
            }
        }
    }
}