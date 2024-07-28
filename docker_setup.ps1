# Setup Docker Services

if (Get-Command docker -ErrorAction SilentlyContinue) {
    Write-Host "Pulling Docker images if missing."
   
    ## Start the vs multi-container
    docker-compose -f "./containers/docker-compose.yml" -p example up -d
}

Write-Host "Head back to  README.md  for deployment of the database and other services..."
