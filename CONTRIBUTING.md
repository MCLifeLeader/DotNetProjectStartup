# Contributing

## Files and Project Structure

The following sections represent the folder structure from the root of the project. This is general instructions on where various project files should be stored and managed.

The project is broken apart into the following examples:
- Api.Startup.Example - This sample application is provided as a demonstration for creating a RESTful API using ASP.NET Core.
- Startup.Blazor.Server - This sample application is provided as a demonstration for creating a Blazor application using ASP.NET Core.
- Console.Startup.Example - This sample application is provided as a demonstration for creating a Windows service application using ASP.NET Core.
- Database.Example - This sample application is provided as a demonstration for creating a database using Database First.
- React.Startup.Example - ToDo
- Startup.Web - This sample application is provided as a demonstration for creating a web application using ASP.NET Core.

### /

Base repository and project configuration files are found here. This includes the gitignore, gitattributes, readme.md and other base files for the git repository. Additionally, other files such as the various PowerShell scripts are support files around getting the project running and docker configuration and setup scripts.

Various project source files should not be stored at this root level and should instead be stored under the '/src' folder.

Lastly, the *.sln file should be found at the root level for convenience in starting your project in Visual Studio.

Various github related actions and configuration files. This also maintains the 'CODEOWNERS' file which puts more granular security controls on the repository. Lastly the 'copilot-instructions.md' file can be updated as needed to help provide additional context about your project to GitHub Copilot.

### /.devcontainer

Dev Container configuration file for setting up a Docker dev container instance.

### /

### /.vscode

VSCode stores configuration details that can be shared between all projects in this folder. The "extensions.json" file provides the Architect or Lead Developer the ability to specify recommended plugins for VSCode. Add / Remove those plugins that will provide maximum benefit to the team.

### /containers

This folder is somewhat devops related, but focuses on the setup and configuration for the dev container. The docker-compose.yml script is to be used to do post setup actions for the dev container and adding any additional resources needed for the project.

For those who are not using the dev container, this should be used to configure your local environment by creating a docker collection of resources for your project. The PowerShell commands './docker_setup.ps1' and './docker_down.ps1' will setup or tear down the docker containers.

### /devops

This is specific for building and deploying the project solution artifacts. Build and deployment yaml scripts are to be stored here and then linked to Azure DevOps for build and deployment. If Terraform resources are needed they should be stored and updated here.

### /src

All project related source code and project files are to be stored and referenced from here.
