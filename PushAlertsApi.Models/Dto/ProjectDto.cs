using System.Linq;

namespace PushAlertsApi.Models.Dto
{
    public class ProjectDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProjectDto(Project project)
        {
            Description = project.Description;
            Uuid = project.Uuid;
            Name = project.Name;
        }

        public static ICollection<ProjectDto> CopyAll(ICollection<Project> dbProjects)
        {
            return dbProjects == null ? new List<ProjectDto>() : dbProjects.Select(p => new ProjectDto(p)).ToList();
        }
    }
}