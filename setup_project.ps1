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

git config --global user.name $userName
git config --global user.email $email
