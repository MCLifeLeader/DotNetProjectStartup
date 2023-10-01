Set-Location .\src\Api\Api.Startup.Example
dotnet build --configuration Development --source .\Api.Startup.Example.csproj
dotnet run .\Api.Startup.Example.csproj --configuration Development --launch-profile https &
Set-Location ..\..\..\
