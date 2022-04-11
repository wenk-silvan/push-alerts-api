using System;
using System.Collections.Generic;
using NUnit.Framework;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using System.Linq;
using MockQueryable.NSubstitute;

namespace PushAlertsApi.Tests
{
    public class TasksServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void AssignTask_Normal()
        {
            // Arrange
            var alice = new User("alice@company.com");
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act 
            tasksService.AssignTask(uuid, alice);
            var actual = tasksService.GetTask(uuid);

            // Assert
            var expected = expectedTasks.First();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(TaskState.Assigned, actual.Status);
            Assert.NotNull(actual.AssignedAt);
            Assert.Greater(actual.AssignedAt, actual.CreatedAt);
            Assert.AreEqual(alice, actual.User);
        }


        [Test]
        public void AssignTask_NoUser()
        {
            // Arrange
            User alice = null;
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tasksService.AssignTask(uuid, alice));
        }


        [Test]
        public void AssignTask_AlreadyAssigned()
        {
            // Arrange
            var alice = new User("alice@company.com");
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();
            tasksService.AssignTask(uuid, alice);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => tasksService.AssignTask(uuid, alice));
        }


        [Test]
        public void AssignTask_AlreadyDone()
        {
            // Arrange
            var alice = new User("alice@company.com");
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();
            tasksService.AssignTask(uuid, alice);
            tasksService.CloseTask(uuid, TaskState.Done);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => tasksService.AssignTask(uuid, alice));
        }


        [Test]
        public void AssignTask_AlreadyRejected()
        {
            // Arrange
            var alice = new User("alice@company.com");
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();
            tasksService.AssignTask(uuid, alice);
            tasksService.CloseTask(uuid, TaskState.Rejected);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => tasksService.AssignTask(uuid, alice));
        }


        [Test]
        public void CloseTask_Done()
        {
            // Arrange
            var alice = new User("alice@company.com");
            var expectedState = TaskState.Done;
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act 
            tasksService.AssignTask(uuid, alice);
            tasksService.CloseTask(uuid, expectedState);
            var actual = tasksService.GetTask(uuid);

            // Assert
            var expected = expectedTasks.First();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedState, actual.Status);
            Assert.NotNull(actual.ClosedAt);
            Assert.Greater(actual.ClosedAt, actual.CreatedAt);
            Assert.Greater(actual.ClosedAt, actual.AssignedAt);
        }

        [Test]
        public void CloseTask_Rejected()
        {
            // Arrange
            var alice = new User("alice@company.com");
            var expectedState = TaskState.Rejected;
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act 
            tasksService.AssignTask(uuid, alice);
            tasksService.CloseTask(uuid, expectedState);
            var actual = tasksService.GetTask(uuid);

            // Assert
            var expected = expectedTasks.First();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedState, actual.Status);
            Assert.NotNull(actual.ClosedAt);
            Assert.Greater(actual.ClosedAt, actual.CreatedAt);
            Assert.Greater(actual.ClosedAt, actual.AssignedAt);
        }

        [Test]
        public void CloseTask_InvalidState()
        {
            // Arrange
            var alice = new User("alice@company.com");
            var expectedState = TaskState.Assigned;
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act & Assert
            tasksService.AssignTask(uuid, alice);
            Assert.Throws<ArgumentException>(() => tasksService.CloseTask(uuid, expectedState));
        }

        [Test]
        public void CloseTask_NotYetAssigned()
        {
            // Arrange
            var alice = new User("alice@company.com");
            var expectedState = TaskState.Done;
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => tasksService.CloseTask(uuid, expectedState));
        }

        [Test]
        public void CloseTask_AlreadyRejected()
        {
            // Arrange
            var alice = new User("alice@company.com");
            var expectedState = TaskState.Done;
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act & Assert
            tasksService.AssignTask(uuid, alice);
            tasksService.CloseTask(uuid, expectedState);
            Assert.Throws<InvalidOperationException>(() => tasksService.CloseTask(uuid, expectedState));
        }

        [Test]
        public void CloseTask_AlreadyDone()
        {
            // Arrange
            var alice = new User("alice@company.com");
            var expectedState = TaskState.Done;
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = expectedTasks.First().Uuid.ToString();

            // Act & Assert
            tasksService.AssignTask(uuid, alice);
            tasksService.CloseTask(uuid, TaskState.Done);
            Assert.Throws<InvalidOperationException>(() => tasksService.CloseTask(uuid, expectedState));
        }

        [Test]
        public void GetTask_Normal()
        {
            // Arrange
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());

            // Act 
            string uuid = expectedTasks.First().Uuid.ToString();
            var actual = tasksService.GetTask(uuid);

            // Assert
            var expected = expectedTasks.First();
            Assert.NotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetTask_InvalidUuid()
        {
            // Arrange
            List<Task> expectedTasks = new()
            {
                new Task("Task A", "Description A", "Unit Test", 1, null),
                new Task("Task B", "Description B", "Unit Test", 1, null)
            };
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = Guid.NewGuid().ToString();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => tasksService.GetTask(uuid));
        }

        [Test]
        public void GetTask_EmptyDb()
        {
            // Arrange
            List<Task> expectedTasks = new();
            var tasksService = new TasksService(expectedTasks.AsQueryable().BuildMockDbSet());
            string uuid = Guid.NewGuid().ToString();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => tasksService.GetTask(uuid));
        }
    }
}