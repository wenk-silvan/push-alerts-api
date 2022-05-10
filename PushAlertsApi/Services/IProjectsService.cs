using PushAlertsApi.Models;

namespace PushAlertsApi.Services
{
    /// <summary>
    /// Serves as a contract for the ProjectsService to meet the business needs.
    /// This service should do CRUD operations to the database context regarding projects.
    /// </summary>
    public interface IProjectsService
    {
        /// <summary>
        /// Returns all projects where the user is part of.
        /// </summary>
        /// <param name="uuidUser">Unique identifier for a user</param>
        /// <returns></returns>
        public ICollection<Project> GetAllProjects(Guid uuidUser);

        /// <summary>
        /// Returns the project with the given Uuid.
        /// </summary>
        /// <param name="uuid">Unique identifier for a project</param>
        /// <returns></returns>
        public Project GetProject(Guid uuid);

        /// <summary>
        /// Returns the project with the given ID.
        /// </summary>
        /// <param name="id">Internal id for a project</param>
        /// <returns></returns>
        public Project GetProject(int id);
    }
}
