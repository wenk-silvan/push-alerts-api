using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using PushAlertsApi.Util;

namespace PushAlertsApi.Controllers
{
    /// <summary>
    /// This Api Controller is responsible for all project related requests.
    /// It is authorized and the endpoints can only be reached if the user is authenticated towards the server.
    /// </summary>
    [ApiController]
    [Authorize]
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

        /// <summary>
        /// Calls the ProjectsService instance to fetch all projects
        /// </summary>
        /// <returns>A list of projects</returns>
        [HttpGet("{uuidUser}")]
        public ActionResult<IEnumerable<Project>> Get(string uuidUser)
        {
            try
            {
                if (!Guid.TryParse(uuidUser, out var uuidUserParsed)) return BadRequest("Invalid uuid");
                return Ok(_projectsService.GetAllProjects(uuidUserParsed));
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