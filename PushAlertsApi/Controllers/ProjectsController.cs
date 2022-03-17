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

        public ProjectsController(ILogger<ProjectsController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            _projectsService = new ProjectsService(context.Projects);
        }

        [HttpGet(Name = "GetProjects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Get()
        {
            return Ok(await _projectsService.GetAllProjects());
        }

        [HttpGet("{uuid}", Name = "GetTasksByProjectUuid")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> Get(string uuid)
        {
            try
            {
                var project = await _context.Projects.FirstAsync(p => p.Uuid == Guid.Parse(uuid));
                var tasks = await _context.Tasks.Where(t => t.ProjectId == project.Id).ToListAsync(); // TODO: Clarify why this statement is needed.
                var result = TaskDto.CopyAll(project.Tasks);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Invalid project UUID");
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Invalid project UUID");
            }
        }

        [HttpPost("{uuid}", Name = "PostTaskByProjectUuid")]
        public async Task<ActionResult<TaskDto>> Post(string uuid, TaskDto task)
        {
            try
            {
                var project = await _context.Projects.FirstAsync(p => p.Uuid == Guid.Parse(uuid));
                var newTask = new Models.Task(
                    task.Title,
                    task.Description,
                    task.Source,
                    task.Payload
                );
                project.Tasks ??= new List<Task>();
                project.Tasks.Add(newTask);
                var x = _context.Projects.Update(project).Entity;
                await _context.SaveChangesAsync();
                return Ok(TaskDto.Copy(newTask));
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Invalid project UUID");
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Invalid project UUID");
            }
        }
    }
}