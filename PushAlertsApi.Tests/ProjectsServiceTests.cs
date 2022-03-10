using NUnit.Framework;
using NSubstitute;
using PushAlertsApi.Models;
using PushAlertsApi.Data;
using PushAlertsApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PushAlertsApi.Tests
{
    public class ProjectsServiceTests
    {
        private readonly Project projectAlpha = new Project
        {
            Description = "This is project Alpha.",
            Id = 1,
            Name = "Alpha",
            Tasks = null,
            Users = null,
            Uuid = System.Guid.Parse("817756de-31f7-4537-844d-6beea0159002")
        };
        private readonly Project projectBeta = new Project
        {
            Description = "This is project Beta.",
            Id = 2,
            Name = "Beta",
            Tasks = null,
            Users = null,
            Uuid = System.Guid.Parse("eb44db73-c2c4-4653-b71c-12462474e0d2")
        };

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetAllProjects_Normal()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("PushAlertsTestDb")
                .Options;
            var context = Substitute.For<DataContext>(options);
            //context.Projects.Returns(new List<Project>());
            context.Projects.Add(projectAlpha);
            context.Projects.Add(projectBeta);
            context.SaveChanges();

            // Act
            var projectsService = new ProjectsService(context);
            var projects = projectsService.GetAllProjects().Result.ToList();

            // Assert
            Assert.IsNotNull(projects);
            Assert.AreEqual(projects.Count, 2);
            Assert.AreEqual(projects.First().Name, projectAlpha.Name);
        }
    }
}
