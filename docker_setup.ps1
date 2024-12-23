# Setup Docker Services

if (Get-Command docker -ErrorAction SilentlyContinue) {
    Write-Host "Docker images and container setup started."

    ## Start the vs multi-container
    docker-compose -f "./containers/docker-compose.yml" -p dotnet_example up -d
    Start-Sleep -Seconds 5
}

Write-Host "Docker images and container setup completed."
Write-Host "Head back to README.md for deployment of the database and other services..."
