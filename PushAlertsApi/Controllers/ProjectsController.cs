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

        private readonly ProjectsService _projectsService;

        public ProjectsController(ILogger<ProjectsController> logger, DataContext context)
        {
            _logger = logger;
            _projectsService = new ProjectsService(context.Projects);
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
                return HandleApiException(ex);
            }
        }

        private ActionResult HandleApiException(Exception ex)
        {
            _logger.LogError(MessageFormatter.LogError(ex));
            return BadRequest(MessageFormatter.ActionResultBadRequest());
        }
    }
}