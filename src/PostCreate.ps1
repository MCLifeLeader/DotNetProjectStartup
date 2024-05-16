# Setup Development Environment DevContainer
Write-Host "Post Create Commands for Environment..."

dotnet workload update
dotnet tool install -g Microsoft.Web.LibraryManager.Cli

cd Api/Api.Startup.Example
dotnet restore Startup.Api.csproj

cd ../../Blazor/Blazor.Server.Startup.Example
dotnet restore Startup.Blazor.Server.csproj
libman restore

cd ../../Console/Console.Startup.Example
dotnet restore Startup.Console.csproj

cd ../../Web/Web.Startup.Example
dotnet restore Startup.Web.csproj
libman restore

cd ../../

# Setup git Configurations
git config --global credential.useHttpPath true
