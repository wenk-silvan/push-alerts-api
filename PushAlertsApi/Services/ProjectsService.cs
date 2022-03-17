using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Data;
using PushAlertsApi.Models;
using PushAlertsApi.Models.Dto;

namespace PushAlertsApi.Services
{
    public class ProjectsService
    {

        private readonly DbSet<Project> _context;

        public ProjectsService(DbSet<Project> context)
        {
            _context = context;
        }
        public async Task<ICollection<ProjectDto>> GetAllProjects()
        {
            return ProjectDto.CopyAll(await _context.ToListAsync());
        }
    }
}
