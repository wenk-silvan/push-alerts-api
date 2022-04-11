using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using PushAlertsApi.Models;
using PushAlertsApi.Services;
using DataContext = PushAlertsApi.Data.DataContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PushAlerts"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-adminsdk-key.json")
    ),
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "V0.1"); });
}

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    using (var context = new DataContext(
               services.GetRequiredService<DbContextOptions<DataContext>>()))
    {
        var service = new ReminderJobService(context.ReminderJobs, 20000);
        service.RemoveOutdatedJobs();
        context.SaveChanges();
        service.ReloadTimerForEachJob((timer, taskId) =>
        {
            var task = new TasksService(context.Tasks).GetTask(taskId);
            var project = new ProjectsService(context.Projects).GetProject(task.ProjectId);
            if (task.Status == TaskState.Opened)
            {
                new NotificationsService(context.Notifications, FirebaseMessaging.DefaultInstance).NotifyUsers(
                    $"Reminder for task '{task.Title}' in project {project.Name}", project, task);
            }
            else
            {
                timer.Dispose();
                context.RemoveRange(context.ReminderJobs.Where(j => j.Task.Id == task.Id));
            }

            context.SaveChanges();
        });

    }
}


app.Run();