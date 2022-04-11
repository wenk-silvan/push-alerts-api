using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using Task = PushAlertsApi.Models.Task;
using Timer = System.Timers.Timer;

namespace PushAlertsApi.Services
{
    public class ReminderJobService
    {
        private readonly ILogger<ReminderJobService> _logger;

        private readonly DbSet<ReminderJob> _dbSet;

        private readonly int _timerIntervalMillis;

        public ReminderJobService(DbSet<ReminderJob> context, int reminderIntervalMillis)
        {
            _logger = new LoggerFactory().CreateLogger<ReminderJobService>();
            _dbSet = context;
            _timerIntervalMillis = reminderIntervalMillis;
        }

        public void ReloadTimerForEachJob(Action<IDisposable, int> onTimerFinished)
        {
            var jobs = _dbSet.ToList();
            jobs.ForEach(j =>
            {
                var remainingMillis = _timerIntervalMillis - (DateTime.Now - j.CreatedAt).TotalMilliseconds;
                var timer = new Timer(remainingMillis);
                timer.Elapsed += (o, args) => onTimerFinished(timer, j.TaskIdForEagerLoading);
                timer.Enabled = true;
            });
        }

        public ReminderJob Add(ReminderJob job, Action<IDisposable, int> onTimerFinished)
        {
            var timer = new Timer(_timerIntervalMillis);
            timer.Elapsed += (o, args) => onTimerFinished(timer, job.TaskIdForEagerLoading);
            timer.Enabled = true;
            _logger.LogDebug($"Add new reminder job with uuid: '{job.Uuid}' for task '{job.Task.Uuid}'");
            return _dbSet.Add(job).Entity;
        }

        public void DeleteAll(Task task)
        {
            var jobs = _dbSet.Where(j => j.Task.Id == task.Id);
            _logger.LogDebug($"Delete {jobs.Count()} reminder jobs with task uuid: '{task.Uuid}'");
            _dbSet.RemoveRange(jobs);
        }

        public void RemoveOutdatedJobs()
        {
            var recently = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(_timerIntervalMillis));
            var jobs = _dbSet.Where(j => j.CreatedAt < recently);
            _logger.LogDebug($"Remove {jobs.Count()} outdated reminder jobs");
            _dbSet.RemoveRange(jobs);
        }
    }
}