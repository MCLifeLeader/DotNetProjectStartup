Set-Location .\src\Blazor\Startup.Blazor.Server
dotnet build --configuration Development --source .\Startup.Blazor.Server.csproj
dotnet run .\Startup.Blazor.Server.csproj --configuration Development --launch-profile https &
Set-Location ..\..\..\
