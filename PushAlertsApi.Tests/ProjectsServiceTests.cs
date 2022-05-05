using System;
using System.Collections.Generic;
using NUnit.Framework;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using System.Linq;
using MockQueryable.NSubstitute;

namespace PushAlertsApi.Tests
{
    public class ProjectsServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAllProjects_Normal()
        {
            var alice = new User("alice@schindler.com");
            var bob = new User("bob@schindler.com");
            // Arrange
            List<Project> expected = new()
            {
                new Project
                {
                    Description = "This is project Alpha.",
                    Id = 1,
                    Name = "Alpha",
                    Tasks = null,
                    Users = new List<User> { alice },
                    Uuid = Guid.Parse("817756de-31f7-4537-844d-6beea0159002")
                },
                new Project
                {
                    Description = "This is project Beta.",
                    Id = 2,
                    Name = "Beta",
                    Tasks = null,
                    Users = new List<User> { bob },
                    Uuid = Guid.Parse("eb44db73-c2c4-4653-b71c-12462474e0d2")
                }
            };

            // Act
            var projectsService = new ProjectsService(expected.AsQueryable().BuildMockDbSet());
            var actual = projectsService.GetAllProjects(alice.Uuid.ToString()).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(expected.First().Uuid, actual.First().Uuid);
            Assert.AreEqual(expected.First().Name, actual.First().Name);
            Assert.AreEqual(expected.First().Description, actual.First().Description);
        }

        [Test]
        public void GetAllProjects_NoProjects()
        {
            // Arrange
            List<Project> expected = new();

            // Act
            var projectsService = new ProjectsService(expected.AsQueryable().BuildMockDbSet());
            var actual = projectsService.GetAllProjects(Guid.NewGuid().ToString()).ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void GetProject_Normal()
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
            var projectsService = new ProjectsService(expected.AsQueryable().BuildMockDbSet());
            var actual = projectsService.GetProject("817756de-31f7-4537-844d-6beea0159002");

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreSame(expected.First(), actual);
            Assert.AreNotSame(expected.Last(), actual);
        }

        [Test]
        public void GetProject_NoProjects()
        {
            // Arrange
            List<Project> expected = new();

            // Act
            var projectsService = new ProjectsService(expected.AsQueryable().BuildMockDbSet());

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
                projectsService.GetProject("817756de-31f7-4537-844d-6beea0159002"));
        }

        [Test]
        public void GetProject_InvalidUuid()
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
                    Uuid = Guid.Parse("817756de-31f7-4537-844d-6beea0159002")
                },
                new Project
                {
                    Description = "This is project Beta.",
                    Id = 2,
                    Name = "Beta",
                    Tasks = null,
                    Users = null,
                    Uuid = Guid.Parse("eb44db73-c2c4-4653-b71c-12462474e0d2")
                }
            };

            // Act
            var projectsService = new ProjectsService(expected.AsQueryable().BuildMockDbSet());

            // Assert
            Assert.Throws<FormatException>(() =>
                projectsService.GetProject("-"));
        }
    }
}