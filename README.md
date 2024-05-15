# DotNetProjectStartup
This project demonstrates one possible way to setup and organize your Visual Studio project. As this project progresses it
will be broken apart into various layers that can be consumed by one of the example project applications provided.

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

# Getting Started 
## Setting up the project
1. Locate the file Dev_Setup.ps1 in the root of the project.
2. Run this file in PowerShell and this will setup the Docker Containers and Dependencies.

## Using SQL Server Express
1. To get started with this project, you will need to clone the repository and then open the solution in Visual Studio. 
2. Once the solution is open, you will need to build the solution. This will download all of the NuGet packages that are required for the project.
3. Open the Database.Example project and build the project then publish using the "StartupExample.publish.xml" profile.
4. There should be no need to update the secrets.json file with the connection string as it should be configured in the appsettings.json file for SQL Express.

## Using Docker Containers
1. To get started with this project, you will need to clone the repository and then open the solution in Visual Studio. 
2. Once the solution is open, you will need to build the solution. This will download all of the NuGet packages that are required for the project.
3. Open the first project and find the "Connected Services" folder. Right click on the "Connected Services" folder and select "Manage Connected Services".
4. Locate the SQL Server Database option and click the elipses button "..." to open the configuration window and select "Open in Containers Window".
5. Locate and select the "mssql" option and click the Environment Tab. Locate the "SA_PASSWORD" option and update your database string in your "Secrets.json" file in each project. 
"ConnectionStrings:DefaultConnection": "Server=localhost,4433;Database=StartupExample;User ID=sa;Password=<DatabasePassword>;Persist Security Info=False;TrustServerCertificate=true;"
6. Open the Database.Example project and build the project then publish using the "StartupExample.Docker.publish.xml" profile. Use the password obtained in step 5.

## Running the Applications
1. Once the project has been built and the database has been created, you can run any of the applications.

# Codespaces
## GitHub Configuration
git config --global user.name "Your Name"
git config --global user.email "youremail@yourdomain.com"
