# Setup Docker Services
## Test to see if Docker is installed
if (Get-Command docker -ErrorAction SilentlyContinue) {
    Write-Host "Docker is installed."
} else {
    Write-Host "Docker is not installed. Installing..."
    Invoke-WebRequest -Uri https://download.docker.com/win/stable/Docker%20Desktop%20Installer.exe -OutFile '\StartupExample\DockerDesktopInstaller.exe'
    Start-Process -FilePath '\StartupExample\DockerDesktopInstaller.exe' -ArgumentList '/quiet' -Wait
    Write-Host "Docker has been installed."
}

if (Get-Command docker -ErrorAction SilentlyContinue) {
    Write-Host "Pulling Docker images if missing."

    ## Pull the Docker images
    docker pull mcr.microsoft.com/azure-storage/azurite
    docker pull mcr.microsoft.com/dotnet/sdk
    docker pull mcr.microsoft.com/dotnet/aspnet
    docker pull mcr.microsoft.com/mssql/server
    docker pull docker.io/library/redis

    ## Stop the StartupExampleSql-Dev container if it is running
    if (docker ps -a | Select-String -Pattern "StartupExampleSql-Dev") {
        Write-Host "Stopping the StartupExampleSql-Dev container..."
        docker stop StartupExampleSql-Dev
        Write-Host "The StartupExampleSql-Dev container has been stopped."
    } else {
        Write-Host "The StartupExampleSql-Dev container is not running."
    }

    ## Remove the StartupExampleSql-Dev container if it exists
    if (docker ps -a | Select-String -Pattern "StartupExampleSql-Dev") {
        Write-Host "Removing the StartupExampleSql-Dev container..."
        docker rm StartupExampleSql-Dev
        Write-Host "The StartupExampleSql-Dev container has been removed."
    } else {
        Write-Host "The StartupExampleSql-Dev container does not exist."
    }

    ## Start the StartupExampleSql-Dev container on port 4433 with local dev password
    docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssword123!' -p 4433:1433 --name 'StartupExampleSql-Dev' -d mcr.microsoft.com/mssql/server
}

Write-Host "Head back to  README.md  for deployment of the database and other services..."
