# Setup Development Environment DevContainer
Write-Host "Post Create Commands for Environment..."

dotnet workload update
dotnet tool install -g Microsoft.Web.LibraryManager.Cli

# Setup git Configurations
git config --global credential.useHttpPath true
git config --global core.autocrlf true
git config --global core.eol crlf

# Install NuGet Support
sudo apt update
sudo apt install -y nuget

# Install .NET Core runtime
sudo apt-get install -y dotnet-sdk-8.0
sudo apt-get install -y dotnet-sdk-9.0

# Trust HTTPS developer certificate
dotnet dev-certs https --trust

Write-Host "run  setup_project.ps1  to complete the process..."