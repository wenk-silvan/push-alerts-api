using System.Linq;

namespace PushAlertsApi.Models.Dto
{
    public class ProjectDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static ICollection<ProjectDto> CopyAll(ICollection<Project> dbProjects)
        {
            return dbProjects == null ?
                new List<ProjectDto>() : dbProjects.Select(Copy).ToList();
        }

        private static ProjectDto Copy(Project dbProject)
        {
            return new ProjectDto
            {
                Description = dbProject.Description,
                Uuid = dbProject.Uuid,
                Name = dbProject.Name
            };
        }
    }
}