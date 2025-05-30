Write-Host "Running tests..."

#Setup dotnet test run argument
[string] $loggerArgument = '"trx;logfilename=Test_Results.' + (Get-Date).ToString("yyyyMMdd.HHmmss") + '.trx"'

# Run the automated tests
Set-Location .\src\Startup.Tests\
dotnet build --configuration Debug --source .\Startup.Tests.csproj
dotnet test .\Startup.Tests.csproj --logger $loggerArgument #--collect:"Code Coverage"

Set-Location ..\..