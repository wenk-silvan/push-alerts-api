using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController: ControllerBase         
    {
        private readonly ILogger<ProjectsController> _logger;

        private static List<Project> _projects = new List<Project>()
        {
            new()
            {
                Description = "This is project Alpha ",
                Name = "Alpha",
                Tasks = CreateFakeTasks(),
                Uuid = Guid.NewGuid()
            },
            new()
            {
                Description = "This is project Beta ",
                Name = "Beta",
                Tasks = CreateFakeTasks(),
                Uuid = Guid.NewGuid()
            }
        };

        public ProjectsController(ILogger<ProjectsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetProjects")]
        public async Task<ActionResult<IEnumerable<Project>>> Get()
        {
            return Ok(_projects);
        }
            
        [HttpGet("{uuid}", Name = "GetProjectByUuid")]
        public async Task<ActionResult<Project>> Get(string uuid)
        {
            var project = _projects.Find(p => p.Uuid.ToString() == uuid);
            if (project == null)
            {
                return BadRequest("Project not found.");
            }

            return Ok(project);
        }

        [HttpPost("{uuid}", Name = "PostTaskToProjectByUuid")]
        public async Task<ActionResult<Project>> Post(string uuid, Task task)
        {
            var project = _projects.Find(p => p.Uuid.ToString() == uuid);
            if (project == null)
            {
                return BadRequest("Project not found.");
            }
            project.Tasks.Add(task);
            return Ok(project);
        }

        private static IList<Task> CreateFakeTasks()
        {
            return Enumerable.Range(1, 2).Select(index => new Models.Task
            {
                    Description = "Lorem ipsum",
                    User = null,
                    Uuid = Guid.NewGuid(),
                    AssignedAt = null,
                    ClosedAt = null,
                    CreatedAt = DateTime.Now,
                    Id = index,
                    Payload = null,
                    Source = "Grafana",
                    Status = TaskState.Opened,
                    Title = "Title"
            })
                .ToList();
        }
    }
}