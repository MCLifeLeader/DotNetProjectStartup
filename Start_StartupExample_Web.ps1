Set-Location .\src\Web\Startup.Web
dotnet build --configuration Development --source .\Startup.Web.csproj
dotnet run .\Startup.Web.csproj --configuration Development --launch-profile https &
Set-Location ..\..\..\
