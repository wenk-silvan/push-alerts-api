using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PushAlertsApi.Data;
using PushAlertsApi.Filters;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using PushAlertsApi.Util;

namespace PushAlertsApi.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        private readonly IProjectsService _projectsService;

        public ProjectsController(ILogger<ProjectsController> logger, DataContext context)
        {
            _logger = logger;
            _projectsService = new ProjectsService(context.Projects);
        }

        [HttpGet, Authorize]
        public ActionResult<IEnumerable<Project>> Get()
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