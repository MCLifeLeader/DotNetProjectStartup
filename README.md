# Project Setup and Configuration

## About this Project

This project demonstrates one possible way to setup and organize your Visual Studio project. As this project progresses it will be broken apart into various layers that can be consumed by one of the example project applications provided.
You can find my ePortfolio [here](https://github.com/MCLifeLeader/ePortfolio).

## Project Structure

The project is broken apart into the following examples:
- Api.Startup.Example - This sample application is provided as a demonstration for creating a RESTful API using ASP.NET Core.
- Startup.Blazor.Server - This sample application is provided as a demonstration for creating a Blazor application using ASP.NET Core.
- Console.Startup.Example - This sample application is provided as a demonstration for creating a Windows service application using ASP.NET Core.
- Database.Example - This sample application is provided as a demonstration for creating a database using Database First.
- React.Startup.Example - ToDo
- Startup.Web - This sample application is provided as a demonstration for creating a web application using ASP.NET Core.

## Project Dependencies
### Required
- .NET
- Visual Studio
- SQL Server Express

### Recommended
- Docker Desktop
- Visual Studio Code
- SQL Server Management Studio

## Getting Started
### Setting up the project
1. Clone the repository to your local machine.
2. Locate the file docker_setup.ps1 in the root of the project and run this in PowerShell which will setup the Docker Containers and Dependencies.

### Using SQL Server Express
1. To get started with this project, you will need to clone the repository and then open the solution in Visual Studio.
2. Once the solution is open, you will want to build the solution. This will download all of the NuGet packages that are required or the project.
3. Open the Database.Example project and build the project then publish using the "StartupExample.publish.xml" profile.
4. There should be no need to update the secrets.json file with the connection string as it should be configured in the appsettings.json file for SQL Express.

### Using Docker Containers
1. Start Visual Studio and open the solution.
2. Open the Database.Example project and build the project then publish using the "StartupExample.Docker.publish.xml" profile. Using the default password of "P@ssword123!".
3. MailHog was added as an email trap.
4. Open Telemetry was added to the project to help with debugging and development and can be found [here](http://localhost:4341/) after starting the docker containers.
   1. Watch the following video for why and how its used: [The Logging Everyone Should Be Using in .NET](https://www.youtube.com/watch?v=MHJ0BHfWhRw)

### Running the Applications
1. Once the project has been built and the database has been created, you can run any of the applications.

## CodeSpaces

If you are using CodeSpaces you'll want to update your container git configuration profile. Be sure to update with your appropriate name and email details.

### GitHub Configuration
```
git config --global user.name "Your Name"
git config --global user.email "youremail@yourdomain.com"
```

## Installing Resources

### DotNet SDKs

✅ Required Resource ✅

```
winget install Microsoft.DotNet.SDK.9
```
### Windows Terminal Console Support

👍 Recommended 👍

```
winget install Microsoft.WindowsTerminal
```

### Latest version of PowerShell

✅ Required Resource ✅

```
winget install Microsoft.PowerShell
```

### Azure toolkit

👍 Recommended 👍

```
winget install Microsoft.AzureCLI
winget install Microsoft.Azd
winget install Microsoft.Azure.FunctionsCoreTools
```

### CommandLine Git Client

✅ Required Resource ✅

```
winget install Git.Git
```

### Visual Git Clients

👍 Recommended 👍

```
winget install Atlassian.Sourcetree
winget install GitHub.GitHubDesktop
```

### Use for Git Merge Conflicts, compares files and folders

👍 Recommended 👍
⭐ License Required ⭐

```
winget install ScooterSoftware.BeyondCompare5
```

### Azure

```
winget install Microsoft.Azure.StorageEmulator
winget install Microsoft.Azure.StorageExplorer
```

### Postman Gui Http Client

👍 Recommended 👍

```
winget install Postman.Postman
```

### Local Database Engine, SQL Server Express

👍 Recommended 👍

```
winget install Microsoft.SQLServer.2022.Express
```

### IDE and Text Editors

✅ Required Resource ✅

```
winget install Microsoft.VisualStudio.2022.Enterprise
winget install Microsoft.VisualStudioCode
```

👍 Recommended 👍

```
winget install Microsoft.SQLServerManagementStudio
winget install Notepad++.Notepad++
```

### Visual Studio Addon Tools Extensions

👍 Recommended 👍
⭐ License Required ⭐

```
winget install JetBrains.Toolbox
winget install JetBrains.ReSharper
```

### Docker / Container Services

👍 Recommended 👍
⭐ License Required ⭐

This resource provides local development tools and resources will speed up developer setup and configuration and provide diagnostic and debugging resources not previously available. The number of options and tools is near limitless.

```
winget install Docker.DockerDesktop
```

### Update Dotnet workloads

👍 Recommended 👍

```
dotnet workload update
dotnet workload install wasm-tools
dotnet workload install aspire
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```