using Microsoft.AspNetCore.Mvc;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController: ControllerBase         
    {
        private readonly ILogger<ProjectsController> _logger;

        private static readonly List<ProjectDto> Projects = new()
        {
            new ProjectDto
            {
                Description = "This is project Alpha ",
                Name = "Alpha",
                Tasks = CreateFakeTasks(),
                Uuid = Guid.NewGuid()
            },
            new ProjectDto
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
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Get()
        {
            return Ok(Projects);
        }
            
        [HttpGet("{uuid}", Name = "GetProjectByUuid")]
        public async Task<ActionResult<ProjectDto>> Get(string uuid)
        {
            var project = Projects.Find(p => p.Uuid.ToString() == uuid);
            if (project == null)
            {
                return BadRequest("Project not found.");
            }

            return Ok(project);
        }

        [HttpPost("{uuid}", Name = "PostTaskToProjectByUuid")]
        public async Task<ActionResult<ProjectDto>> Post(string uuid, TaskDto task)
        {
            var project = Projects.Find(p => p.Uuid.ToString() == uuid);
            if (project == null)
            {
                return BadRequest("Project not found.");
            }
            project.Tasks.Add(task);
            return Ok(project);
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