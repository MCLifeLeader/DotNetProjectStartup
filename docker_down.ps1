# Teardown Docker Services

if (Get-Command docker -ErrorAction SilentlyContinue) {
    Write-Host "Pulling Docker images if missing."

    ## Teardown the vs multi-container
    docker-compose -f "./containers/docker-compose.yml" -p example down
}

Write-Host "Docker Teardown Complete..."
