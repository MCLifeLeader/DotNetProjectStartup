# Setup Docker Services

if (Get-Command docker -ErrorAction SilentlyContinue) {
    Write-Host "Docker images and container setup started."

    ## Start the vs multi-container
    docker-compose -f "./containers/docker-compose-common.yml" -p common_shared up -d
    #docker-compose -f "./containers/docker-compose.yml" -p project up -d
}

Write-Host "Docker images and container setup completed."
Write-Host "Head back to README.md for deployment of the database and other services..."
