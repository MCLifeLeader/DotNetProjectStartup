Set-Location .\src\Web\Web.Startup.Example
dotnet build --configuration Development --source .\Web.Startup.Example.csproj
dotnet run .\Web.Startup.Example.csproj --configuration Development --launch-profile https &
Set-Location ..\..\..\
