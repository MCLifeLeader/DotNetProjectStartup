# DotNetProjectStartup
This project demonstrates one possible way to setup and organize your Visual Studio project. As this project progresses it will be broken apart into various layers that can be consumed by one of the example project applications provided.

## ePortfolio
You can find my ePortfolio [here](https://github.com/MCLifeLeader/ePortfolio).

# Project Structure
The project is broken apart into the following examples:
- Api.Startup.Example - This sample application is provided as a demonstration for creating a RESTful API using ASP.NET Core.
- Startup.Blazor.Server - This sample application is provided as a demonstration for creating a Blazor application using ASP.NET Core.
- Console.Startup.Example - This sample application is provided as a demonstration for creating a Windows service application using ASP.NET Core.
- Database.Example - This sample application is provided as a demonstration for creating a database using Database First.
- React.Startup.Example - ToDo
- Startup.Web - This sample application is provided as a demonstration for creating a web application using ASP.NET Core.

# Project Dependencies
## Required
- .NET 8.0
- Visual Studio 2022
- SQL Server Express 2019 or later

## Recommended
- SQL Server Management Studio or other SQL Server management tool
- Docker Desktop

# Getting Started
## Setting up the project
1. Clone the repository to your local machine.
2. Locate the file docker_setup.ps1 in the root of the project and run this in PowerShell which will setup the Docker Containers and Dependencies.

## Using SQL Server Express
1. To get started with this project, you will need to clone the repository and then open the solution in Visual Studio.
2. Once the solution is open, you will need to build the solution. This will download all of the NuGet packages that are required or the project.
3. Open the Database.Example project and build the project then publish using the "StartupExample.publish.xml" profile.
4. There should be no need to update the secrets.json file with the connection string as it should be configured in the ppsettings json file for SQL Express.

## Using Docker Containers
1. Start Visual Studio and open the solution.
2. Open the Database.Example project and build the project then publish using the "StartupExample.Docker.publish.xml" profile. Using the default password of "P@ssword123!".
3. MailHog was added as an email trap.
4. Open Telemetry was added to the project to help with debugging and development and can be found [here](http://localhost:4341/) after starting the docker containers.


## Running the Applications
1. Once the project has been built and the database has been created, you can run any of the applications.

# Codespaces
## GitHub Configuration
git config --global user.name "Your Name"
git config --global user.email "youremail@yourdomain.com"
