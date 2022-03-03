using Microsoft.AspNetCore.Mvc;
using PushAlertsApi.Models;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController: ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetProjects")]
        public IEnumerable<Project> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Project
            {
                Description = "",
                Name = index.ToString(),
                Tasks = CreateFakeTasks(),
                Uuid = Guid.NewGuid()
            })
            .ToArray();
        }

        private IEnumerable<Models.Task> CreateFakeTasks()
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
                .ToArray();
        }
    }
}