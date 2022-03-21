using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using PushAlertsApi.Controllers;
using PushAlertsApi.Models.Dto;

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
            var actual = projectsService.GetAllProjects().Result.ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected.First().Uuid, actual.First().Uuid);
            Assert.AreEqual(expected.First().Name, actual.First().Name);
            Assert.AreEqual(expected.First().Description, actual.First().Description);
            Assert.AreEqual(expected.Last().Uuid, actual.Last().Uuid);
            Assert.AreEqual(expected.Last().Name, actual.Last().Name);
            Assert.AreEqual(expected.Last().Description, actual.Last().Description);
        }

        [Test]
        public void GetAllProjects_NoProjects()
        {
            // Arrange
            List<Project> expected = new();

            // Act
            var projectsService = new ProjectsService(expected.AsQueryable().BuildMockDbSet());
            var actual = projectsService.GetAllProjects().Result.ToList();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Count, 0);
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
            var actual = projectsService.GetProject("817756de-31f7-4537-844d-6beea0159002").Result;

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
            Assert.ThrowsAsync<TargetInvocationException>(() =>
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
            Assert.ThrowsAsync<TargetInvocationException>(() =>
                projectsService.GetProject("-"));
        }
    }
}