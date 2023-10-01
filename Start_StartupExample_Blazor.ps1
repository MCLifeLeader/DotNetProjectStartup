Set-Location .\src\Blazor\Blazor.Startup.Example
dotnet build --configuration Development --source .\Blazor.Startup.Example.csproj
dotnet run .\Blazor.Startup.Example.csproj --launch-profile https &
Set-Location ..\..\..\
