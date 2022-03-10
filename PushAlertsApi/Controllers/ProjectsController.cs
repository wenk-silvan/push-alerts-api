using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        private static readonly List<ProjectDto> Projects = new()
        {
            new ProjectDto
            {
                Description = "This is project Alpha ",
                Name = "Alpha",
                Uuid = Guid.NewGuid()
            },
            new ProjectDto
            {
                Description = "This is project Beta ",
                Name = "Beta",
                Uuid = Guid.NewGuid()
            }
        };

        private readonly DataContext _context;

        public ProjectsController(ILogger<ProjectsController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetProjects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Get()
        {
            return Ok(ProjectDto.CopyAll(await _context.Projects.ToListAsync()));
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

        private static List<TaskDto> CreateFakeTasks()
        {
            return Enumerable.Range(1, 2).Select(index => new TaskDto
                {
                    Description = "Lorem ipsum",
                    User = null,
                    Uuid = Guid.NewGuid(),
                    AssignedAt = null,
                    ClosedAt = null,
                    CreatedAt = DateTime.Now,
                    Payload = null,
                    Source = "Grafana",
                    Status = TaskState.Opened,
                    Title = "Title"
                })
                .ToList();
        }
    }
}