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
    public class TasksServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //[Test]
        //public void AddTask_Normal()
        //{
        //    // Arrange
        //    List<Task> expectedTasks = new()
        //    {
        //        new Task
        //        {
        //            Title = "Task A",
        //            Description = "Description A",
        //            Source = "Unit Test",
        //            Payload = null,
        //            ProjectId = 1
        //        },
        //        new Task
        //        {
        //            Title = "Task B",
        //            Description = "Description B",
        //            Source = "Unit Test",
        //            Payload = null,
        //            ProjectId = 1
        //        }
        //    };

        //    var expectedProject = new Project
        //    {
        //        Description = "This is project Alpha.",
        //        Id = 1,
        //        Name = "Alpha",
        //        Tasks = expectedTasks,
        //        Users = null,
        //        Uuid = Guid.Parse("817756de-31f7-4537-844d-6beea0159002")
        //    };

        //    var newTask = new TaskDto
        //    {
        //        Title = "My new Task",
        //        Description = "A description of my new Task",
        //        Source = "Unit Test",
        //    };

        //    // Act
        //    var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
        //    var actualTask = tasksService.AddTask(expectedProject, newTask).Result;
        //    var actualTasks = tasksService
        //        .GetTasks(expectedProject).Result;

        //    // Assert
        //    Assert.AreEqual(3, actualTasks.Count);
        //    Assert.AreEqual(newTask.Source, actualTask.Source);
        //    Assert.AreEqual(newTask.Title, actualTask.Title);
        //    Assert.AreEqual(newTask.Description, actualTask.Description);
        //    Assert.AreEqual(newTask.Source, actualTasks.Last().Source);
        //    Assert.AreEqual(newTask.Title, actualTasks.Last().Title);
        //    Assert.AreEqual(newTask.Description, actualTasks.Last().Description);
        //}

        //[Test]
        //public void AddTask_NoTasks()
        //{
        //    // Arrange
        //    List<Task> expectedTasks = new();

        //    var expectedProject = new Project
        //    {
        //        Description = "This is project Alpha.",
        //        Id = 1,
        //        Name = "Alpha",
        //        Tasks = expectedTasks,
        //        Users = null,
        //        Uuid = Guid.Parse("817756de-31f7-4537-844d-6beea0159002")
        //    };

        //    var newTask = new TaskDto
        //    {
        //        Title = "My new Task",
        //        Description = "A description of my new Task",
        //        Source = "Unit Test",
        //    };

        //    // Act
        //    var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
        //    var actualTask = tasksService.AddTask(expectedProject, newTask).Result;
        //    var actualTasks = tasksService
        //        .GetTasks(expectedProject).Result;

        //    // Assert
        //    Assert.AreEqual(1, actualTasks.Count);
        //    Assert.AreEqual(newTask.Source, actualTask.Source);
        //    Assert.AreEqual(newTask.Title, actualTask.Title);
        //    Assert.AreEqual(newTask.Description, actualTask.Description);
        //    Assert.AreEqual(newTask.Source, actualTasks.First().Source);
        //    Assert.AreEqual(newTask.Title, actualTasks.First().Title);
        //    Assert.AreEqual(newTask.Description, actualTasks.First().Description);
        //}
    }
}