# Push Alerts API
This repository is part of the Push Alerts system. It provides an application programming interface (api) 
with the functionalities of interacting with the database. The most important components are:

- returning projects containing the tasks
- authenticating the mobile users
- handling push notification broadcasts
- creation of new tasks

Since the application was built using .NET6 it can be run on Linux, macOS and Windows.

## Configuration
This section explains the necessary steps for an initial setup of the application.

### Webserver
1. Create new Firebase project
2. Setup Cloud Messaging in the Firebase Console
3. Generate new private key in Firebase service accounts, download the key json file and save it as `./PushAlertsApi/firebase-adminsdk-key.json`.
4. Create `./PushAlertsApi/appsettings.json` with the following content:
```
{
  "ConnectionStrings": {
    "PushAlerts": "server=[DB_HOST];database=pushalerts;User Id=[DB_USER];Password=[DB_PASSWORD];"
  },
  "ApiKey": "[API_KEY]",
  "ReminderSeconds": [REMINDER_SECONDS],
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "[JWT_KEY]",
    "Issuer": "https://localhost:44355/",
    "Audience": "https://localhost:44355/",
    "Days": [EXPIRY_DAYS]
  }
}
```
- [DB_HOST] Host name of the database server e.g. localhost
- [DB_USER] Name of the database user
- [DB_PASSWORD] Value of the password for the database user
- [API_KEY] Any combination of characters serving as key for certain API requests e.g. 55e07bc1-c405-4341-abe8-916666e4f8be
- [REMINDER_SECONDS] The amount of seconds after which a reminder is sent for unassigned tasks. If not set the default is 1800
- [JWT_KEY] Any combination of characters serving as symmetric key to create JSON Web Tokens e.g. 55e07bc1-c405-4341-abe8-916666e4f8be
- [EXPIRY_DAYS] The amount of days after the JSON Web Token becomes expired and the user should automatically be logged out of the client app

### Database
1. Install SQL Server or SQL Server Express with the basic configuration
2. Create new user
3. Configure connection string in `appsettings.json`
4. Install the EF Core tools (https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
5. Create a database migration `sudo dotnet ef migrations add Initial`
6. Execute the database migration `sudo dotnet ef database update`
7. The database and the necessary tables should now exists 

## Running the application
Make sure to have completed all necessary configuration steps.

### Using Visual Studio
1. Open the solution file PushAlertsApi.sln in Visual Studio 2019 or 2022
2. Set PushAlertsApi.csproj as startup project
3. Run the application

### Using dotnet CLI
Execute in terminal: `dotnet run --urls=http://[HOST]:[PORT]/ --project= [PATH]/push-alerts-api/PushAlertsApi/PushAlertsApi.csproj`

Example execution: `dotnet run --urls=http://127.0.0.1:80/ --project=/var/run/push-alerts-api/PushAlertsApi/PushAlertsApi.csproj`

## .NET Solution
The solution contains three .NET Core projects.

### PushAlertsApi
ASP.NET Core Web API which exposes the HTTP REST interface.

### PushAlertsApi.Models
.NET Core Class Library which contains all necessary data classes to operate with the data.

### PushAlertsApi.Tests
NUnit project to run unit and integration tests.

## Documentation
This project integrates the OpenAPI standard with Swagger. A detailed documentation is available when running the project and opening the browser on `http://localhost:5032/swagger/index.html`.

## Authors
- Silvan Wenk [silvan.wenk@gmail.com]
