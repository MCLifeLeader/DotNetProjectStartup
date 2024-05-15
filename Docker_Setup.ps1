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

    ## Stop the StartupExampleSql container if it is running
    if (docker ps -a | Select-String -Pattern "StartupExampleSql") {
        Write-Host "Stopping the StartupExampleSql container..."
        docker stop StartupExampleSql
        Write-Host "The StartupExampleSql container has been stopped."
    } else {
        Write-Host "The StartupExampleSql container is not running."
    }

    ## Remove the StartupExampleSql container if it exists
    if (docker ps -a | Select-String -Pattern "StartupExampleSql") {
        Write-Host "Removing the StartupExampleSql container..."
        docker rm StartupExampleSql
        Write-Host "The StartupExampleSql container has been removed."
    } else {
        Write-Host "The StartupExampleSql container does not exist."
    }

    ## Start the vs multi-container
    docker-compose.exe -f ".\containers\docker-compose.yml" -p vs up -d
}

Write-Host "Head back to  README.md  for deployment of the database and other services..."
