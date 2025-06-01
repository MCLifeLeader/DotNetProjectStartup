# Create and update .npmrc file
param (
    [string]$email,
    [string]$firstName,
    [string]$lastName
)

if (-not $email -or -not $firstName -or -not $lastName) {
    Write-Output "Usage: .\setup_project.ps1 <email> <firstName> <lastName>"
    exit
}

$userName = $firstName + ' ' + $lastName

Write-Output "Configuring global git env settings..."

git config --global user.name $userName
git config --global user.email $email
git config --global credential.useHttpPath true

Write-Output "Download nuget.exe and configure..."

New-Item -ItemType Directory -Path "\Tools" -Force
Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile "\Tools\nuget.exe"
