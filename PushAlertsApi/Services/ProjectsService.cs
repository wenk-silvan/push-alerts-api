using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Data;
using PushAlertsApi.Models.Dto;

namespace PushAlertsApi.Services
{
    public class ProjectsService
    {

        private readonly DataContext _context;

        public ProjectsService(DataContext context)
        {
            _context = context;
        }
        public async Task<ICollection<ProjectDto>> GetAllProjects()
        {
            return ProjectDto.CopyAll(await _context.Projects.ToListAsync());
        }
    }
}
