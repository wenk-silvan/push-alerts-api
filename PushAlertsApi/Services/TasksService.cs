﻿using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;

namespace PushAlertsApi.Services
{
    /// <summary>
    /// See interface for code documentation
    /// </summary>
    public class TasksService : ITasksService
    {
        private readonly ILogger<TasksService> _logger;

        private readonly DbSet<Task> _dbSet;

        public TasksService(DbSet<Task> context)
        {
            _logger = new LoggerFactory().CreateLogger<TasksService>();
            _dbSet = context;
        }

        public Task AddTask(Project project, NewTask task)
        {
            var newTask = new Task(task.Title, task.Description, task.Source, project.Id, task.Payload);
            var result = _dbSet.Add(newTask);
            _logger.LogInformation($"Added new task with uuid: '{newTask.Uuid}' to project with uuid: '{project.Uuid}'");
            return result.Entity;
        }

        public void AssignTask(Guid uuid, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var dbTask = GetTask(uuid);
            if (dbTask.Status != TaskState.Opened)
                throw new InvalidOperationException($"Can't assign task with Status: '{dbTask.Status}'");
            dbTask.AssignedAt = DateTime.Now;
            dbTask.Status = TaskState.Assigned;
            dbTask.User = user;
            _dbSet.Update(dbTask);
            _logger.LogInformation($"Assigned task with uuid: '{uuid}' to user with uuid: '{user.Uuid}'");
        }

        public void CloseTask(Guid uuid, TaskState status)
        {
            if (status != TaskState.Done && status != TaskState.Rejected)
            {
                var error =
                    $"The provided task state must be either {TaskState.Done} or {TaskState.Rejected}: '{status}'";
                _logger.LogDebug(error);
                throw new ArgumentException(error);
            }

            var dbTask = GetTask(uuid);
            if (dbTask.Status != TaskState.Assigned)
            {
                throw new InvalidOperationException($"Can't close a task with uuid: '{uuid} 'which is not Assigned");
            }

            dbTask.Status = status;
            dbTask.ClosedAt = DateTime.Now;
            _logger.LogInformation($"Closed task with uuid: '{uuid}' and set state to: '{dbTask.Status}'");
        }

        public Task GetTask(Guid uuid)
        {
            var task = _dbSet.First(t => t.Uuid == uuid);
            _logger.LogInformation($"Fetched one task from DB for uuid: '{uuid}' with id: '{task.Id}'");
            return task;
        }
    }
}