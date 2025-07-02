# Teardown Docker Services

if (Get-Command docker -ErrorAction SilentlyContinue) {
    Write-Host "Tearing down containers."

    ## Teardown the vs multi-container
    docker-compose -f "./containers/docker-compose-common.yml" -p common_shared down
    #docker-compose -f "./containers/docker-compose.yml" -p project down
}

Write-Host "Docker Teardown Complete..."
