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
            var projectsService = new ProjectsService(_logger, expected.AsQueryable().BuildMockDbSet());
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
            var projectsService = new ProjectsService(_logger, expected.AsQueryable().BuildMockDbSet());
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
            var projectsService = new ProjectsService(_logger, expected.AsQueryable().BuildMockDbSet());

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
            var projectsService = new ProjectsService(_logger, expected.AsQueryable().BuildMockDbSet());

            // Assert
            Assert.ThrowsAsync<TargetInvocationException>(() =>
                projectsService.GetProject("-"));
        }

        [Test]
        public void AddTask_Normal()
        {
            List<Task> expectedTasks = new()
            {
                new Task
                {
                    Title = "Task A",
                    Description = "Description A",
                    Source = "Unit Test",
                    Payload = null,
                    ProjectId = 1
                },
                new Task
                {
                    Title = "Task B",
                    Description = "Description B",
                    Source = "Unit Test",
                    Payload = null,
                    ProjectId = 1
                }
            };

            // Arrange
            List<Project> expectedProjects = new()
            {
                new Project
                {
                    Description = "This is project Alpha.",
                    Id = 1,
                    Name = "Alpha",
                    Tasks = expectedTasks,
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

            var newTask = new TaskDto
            {
                Title = "My new Task",
                Description = "A description of my new Task",
                Source = "Unit Test",
            };

            // Act
            var projectsService = new ProjectsService(_logger, expectedProjects.AsQueryable().BuildMockDbSet());
            var tasksService = new TasksService(_logger, expectedTasks.AsQueryable().BuildMockDbSet());
            var actualTask = projectsService.AddTask("817756de-31f7-4537-844d-6beea0159002", newTask).Result;
            var actualTasks = tasksService.GetTasks(projectsService.GetProject("817756de-31f7-4537-844d-6beea0159002").Result).Result;

            // Assert
            Assert.AreEqual(3, actualTasks.Count);
            Assert.AreEqual(newTask.Source, actualTask.Source);
            Assert.AreEqual(newTask.Title, actualTask.Title);
            Assert.AreEqual(newTask.Description, actualTask.Description);
            Assert.AreEqual(newTask.Source, actualTasks.Last().Source);
            Assert.AreEqual(newTask.Title, actualTasks.Last().Title);
            Assert.AreEqual(newTask.Description, actualTasks.Last().Description);
        }

        [Test]
        public void AddTask_NoTasks()
        {
            // Arrange
            List<Project> expectedProjects = new()
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

            List<Task> expectedTasks = new();
            var newTask = new TaskDto
            {
                Title = "My new Task",
                Description = "A description of my new Task",
                Source = "Unit Test",
            };

            // Act
            var projectsService = new ProjectsService(_logger, expectedProjects.AsQueryable().BuildMockDbSet());
            var tasksService = new TasksService(_logger, expectedTasks.AsQueryable().BuildMockDbSet());
            var actualTask = projectsService.AddTask("817756de-31f7-4537-844d-6beea0159002", newTask).Result;
            var actualTasks = tasksService.GetTasks(projectsService.GetProject("817756de-31f7-4537-844d-6beea0159002").Result).Result;

            // Assert
            Assert.AreEqual(1, actualTasks.Count);
            Assert.AreEqual(newTask.Source, actualTask.Source);
            Assert.AreEqual(newTask.Title, actualTask.Title);
            Assert.AreEqual(newTask.Description, actualTask.Description);
            Assert.AreEqual(newTask.Source, actualTasks.First().Source);
            Assert.AreEqual(newTask.Title, actualTasks.First().Title);
            Assert.AreEqual(newTask.Description, actualTasks.First().Description);
        }

        [Test]
        public void AddTask_InvalidUuid()
        {
            // Arrange
            List<Project> expectedProjects = new()
            {
                new Project
                {
                    Description = "This is project Alpha.",
                    Id = 1,
                    Name = "Alpha",
                    Tasks = new()
                    {
                        new Task
                        {
                            Title = "Task A",
                            Description = "Description A",
                            Source = "Unit Test",
                            Payload = null,
                            ProjectId = 1
                        },
                        new Task
                        {
                            Title = "Task B",
                            Description = "Description B",
                            Source = "Unit Test",
                            Payload = null,
                            ProjectId = 1
                        }
                    },
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

            List<Task> expectedTasks = new()
            {
                new Task
                {
                    Title = "Task A",
                    Description = "Description A",
                    Source = "Unit Test",
                    Payload = null,
                    ProjectId = 1
                },
                new Task
                {
                    Title = "Task B",
                    Description = "Description B",
                    Source = "Unit Test",
                    Payload = null,
                    ProjectId = 1
                }
            };

            var newTask = new TaskDto
            {
                Title = "My new Task",
                Description = "A description of my new Task",
                Source = "Unit Test",
            };

            // Act
            var projectsService = new ProjectsService(_logger, expectedProjects.AsQueryable().BuildMockDbSet());
            var tasksService = new TasksService(_logger, expectedTasks.AsQueryable().BuildMockDbSet());

            // Assert
            Assert.ThrowsAsync<TargetInvocationException>(() =>
                projectsService.AddTask("-", newTask));
        }
    }
}