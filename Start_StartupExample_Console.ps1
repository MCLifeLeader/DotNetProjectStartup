Set-Location .\src\Console\Console.Startup.Example
dotnet build --configuration Development --source .\Console.Startup.Example.csproj
dotnet run .\Console.Startup.Example.csproj --configuration Development --launch-profile https &
Set-Location ..\..\..\
