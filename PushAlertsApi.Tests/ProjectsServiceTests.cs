using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using System.Linq;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using PushAlertsApi.Controllers;

namespace PushAlertsApi.Tests
{
    public class ProjectsServiceTests
    {
        private readonly ILogger<ProjectsController> _logger = new LoggerFactory().CreateLogger<ProjectsController>();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAllProjects_Normal()
        {
            // Arrange
            List<Project> expected = new()
            {
                new Project
                {
                    Description = "This is project Alpha.",
                    Id = 1,
                    Name = "Alpha",
                    Tasks = null,
                    Users = null,
                    Uuid = System.Guid.Parse("817756de-31f7-4537-844d-6beea0159002")
                },
                new Project
                {
                    Description = "This is project Beta.",
                    Id = 2,
                    Name = "Beta",
                    Tasks = null,
                    Users = null,
                    Uuid = System.Guid.Parse("eb44db73-c2c4-4653-b71c-12462474e0d2")
                }
            };

            // Act
            var projectsService = new ProjectsService(_logger, expected.AsQueryable().BuildMockDbSet());
            var actual = projectsService.GetAllProjects().Result.ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Count, 2);
            Assert.AreEqual(actual.First().Name, actual.First().Name);
        }

        [Test]
        public void GetAllProjects_Empty()
        {
            // Arrange
            List<Project> expected = new();

            // Act
            var projectsService = new ProjectsService(_logger, expected.AsQueryable().BuildMockDbSet());
            var actual = projectsService.GetAllProjects().Result.ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Count, 0);
        }
    }
}