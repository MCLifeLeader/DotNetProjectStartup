[string] $serviceName = $args[0]
[string] $directoryPath = $args[1]
[string] $lane = $args[2]
[string] $sourcePath = $PSScriptRoot
[string] $binPath = $directoryPath + '\' + $args[0] + '.exe'

# Install_Service.ps1 Console.Startup.Example C:\Services\Example Producation

# If lane is not null, append it to the service name
if ($null -ne $lane) {
    $serviceName = $serviceName + '-' + $lane
}

Write-Host "This script will register or update the existing service registration for the windows application service."
Write-Host ""
Write-Host "** Please verify that the AD - Identity that this service is running under is correct after deployment. **"
Write-Host ""
Write-Host "Configuration: "
Write-Host "ServiceName=" $serviceName
Write-Host "DirectoryPath=" $directoryPath
Write-Host "SourcePath=" $sourcePath
Write-Host "ServiceExe=" $binPath
Write-Host ""

if (Test-Path $directoryPath -PathType Container) {
    Write-Host "The $directoryPath directory already exists."
} else {
    Write-Host "The $directoryPath directory does not exist. Creating..."
    New-Item -Path $directoryPath -ItemType Directory
    Write-Host "The $directoryPath directory has been created."
}

if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    Write-Host "The $serviceName service already exists. Stopping..."
    Stop-Service -Name $serviceName
    Start-Sleep -Milliseconds 10000
} 

if (Test-Path $directoryPath -PathType Container) {
    Write-Host "Deleting files from $directoryPath"
    $scArgs = $directoryPath + '\*'
    Remove-Item -Path $scArgs -Recurse -Force

    Write-Host "Copying files from $sourcePath to $directoryPath..."
    Get-ChildItem -Path $sourcePath -File | ForEach-Object {
        $destinationFilePath = Join-Path -Path $directoryPath -ChildPath $_.Name
        Copy-Item -Path $_.FullName -Destination $destinationFilePath -Force
        Write-Host $destinationFilePath
        Unblock-File $destinationFilePath
    }
    Write-Host "Files have been copied to $directoryPath."
    Write-Host ""
} else {
    Write-Host "The $directoryPath directory does not exist. Please create it before copying files."
}

if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    Write-Host "The $serviceName service already exists. Stopping..."
    Stop-Service -Name $serviceName
    Start-Sleep -Milliseconds 10000
} else {
    Write-Host "The $serviceName service does not exist. Registering..."

    $scArgs = "create $serviceName binPath=`"$binPath`" start=auto"
    Start-Process -FilePath "sc.exe" -ArgumentList $scArgs -Wait

    Write-Host "The $serviceName service has been registered at $binPath location."
}

if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    Write-Host "Starting $serviceName service..."
    Start-Service -Name $serviceName
    Write-Host "$serviceName service has been started."
} else {
    Write-Host "$serviceName service does not exist."
}
