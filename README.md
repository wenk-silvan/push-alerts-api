# Push Alerts API
This repository is part of the Push Alerts system. It provides an application programming interface (api) 
with the functionalities of interacting with the database. The most important components are:

- returning projects containing the tasks
- authenticating the mobile users
- handling push notification broadcasts
- creation of new tasks

## Solution
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
- Silvan Wenk
