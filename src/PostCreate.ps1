# Setup Development Environment DevContainer
Write-Host "Post Create Commands for Environment..."

cd Api/Api.Startup.Example
dotnet restore Startup.Api.csproj
cd ../../Blazor/Blazor.Server.Startup.Example
dotnet restore Startup.Blazor.Server.csproj
cd ../../Console/Console.Startup.Example
dotnet restore Startup.Console.csproj
cd ../../Web/Web.Startup.Example
dotnet restore Startup.Web.csproj
cd ../../

# Setup git Configurations
git config --global credential.useHttpPath true
