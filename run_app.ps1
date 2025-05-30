[string] $launch_profile = $args[0]

if ($null -eq $launch_profile -or $launch_profile -eq "") {
    Write-Host "* * * *"
    Write-Host "Valid arguments are 'http', 'https', or 'httpsalt'. Defaulting to 'https'."
    Write-Host "* * * *"
    $launch_profile = "https"    
}

Write-Host "Starting Application Services..."

Set-Location .\src\Api\Api.Startup.Example
dotnet build --configuration Development --source .\Api.Startup.Example.csproj
dotnet run .\Api.Startup.Example.csproj --configuration Development --launch-profile $launch_profile &

Set-Location ..\..\..\
