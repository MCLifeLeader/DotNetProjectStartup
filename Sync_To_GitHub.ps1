[string] $git_repo_name = $args[0]
[string] $git_pat = $args[1]
[string] $ado_project = $args[2]
[string] $ado_repo_name = $args[3]
[string] $ado_pat = $args[4]
[string] $git_url = 'https://' + $git_pat + '@github.com/' + $git_repo_name
[string] $ado_url = 'https://' + $ado_pat + '@dev.azure.com/agameempowerment/' + $ado_project + '/_git/' + $ado_repo_name
[string] $directoryPath = $ado_repo_name + '.git'

Write-Host "One way sync from Azure Repos to GitHub"
Write-Host ""
Write-Host "Configuration: "
Write-Host "git_repo_name =" $git_repo_name
Write-Host ""

if (Test-Path $directoryPath -PathType Container) {
    Write-Host "Deleting files from $directoryPath"
    $scArgs = $directoryPath + '\*'
    Remove-Item -Path $scArgs -Recurse -Force
} else {
    Write-Host "The $directoryPath directory does not exist."
}

git clone --mirror $ado_url
cd $directoryPath
git remote add --mirror=fetch secondary $git_url
git fetch origin
git push secondary --all --force
cd ..
