# Setup Development Environment DevContainer
Write-Host "Post Create Commands for Environment..."

# Update the system
sudo apt update
sudo apt upgrade -y

dotnet workload update
dotnet tool install -g Microsoft.Web.LibraryManager.Cli

# Setup git Configurations
git config --global credential.useHttpPath true
git config --global core.autocrlf input
#git config --global core.eol lf

# Install Package Manager Support
sudo apt install -y nuget
# sudo apt install -y npm

# Install .NET Core runtime
# sudo apt-get install -y dotnet-sdk-8.0
# sudo apt-get install -y dotnet-sdk-9.0

# Trust HTTPS developer certificate
dotnet dev-certs https --trust

Write-Host "run  Setup_project.ps1  to complete the process..."