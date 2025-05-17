try
{
    # Setup the location for all of the DLLs to be copied to
    Write-Host "Creating Target Folders if they do not exist"
    New-Item -ItemType Directory -Force -Path ".\..\nugetlibs"
    New-Item -ItemType Directory -Force -Path ".\..\nugetlibs\net8.0"
    New-Item -ItemType Directory -Force -Path ".\..\nugetlibs\net9.0"

    # Clean the target location
    Write-Host "Cleaning Target Folders"
    Remove-Item ".\..\nugetlibs\*.dll"
    Remove-Item ".\..\nugetlibs\*.pdb"
    Remove-Item ".\..\nugetlibs\net8.0\*.dll"
    Remove-Item ".\..\nugetlibs\net8.0\*.pdb"
    Remove-Item ".\..\nugetlibs\net9.0\*.dll"
    Remove-Item ".\..\nugetlibs\net9.0\*.pdb"

    # Copy the various DLLs to the nugetlibs folder for processing
    Write-Host "Copying DLLs and PDBs to Target Folders"
    $filesToCopy = Get-ChildItem -Path .\..\ -Include "Startup.Common.*.dll","Startup.Common.*.pdb" -Recurse -Force
    foreach($file in $filesToCopy){
        $fileName = $($file.FullName)

        # Write-Host "Copying $fileName"

        if($fileName -match "net8.0"){
            Copy-Item $fileName ".\..\nugetlibs\net8.0\" -Force -ErrorAction SilentlyContinue
        }
        if($fileName -match "net9.0"){
            Copy-Item $fileName ".\..\nugetlibs\net9.0\" -Force -ErrorAction SilentlyContinue
        }
    }

    # Validate that the files were copied as output to the log when run in Azure DevOps
    Write-Host "Validate Target Folder Contents"
    Get-ChildItem -Path ".\..\nugetlibs\"
    Get-ChildItem -Path ".\..\nugetlibs\netstandard2.0\"
    Get-ChildItem -Path ".\..\nugetlibs\netstandard2.1\"
    Get-ChildItem -Path ".\..\nugetlibs\net6.0\"
    Get-ChildItem -Path ".\..\nugetlibs\net7.0\"
    Get-ChildItem -Path ".\..\nugetlibs\net8.0\"
    Get-ChildItem -Path ".\..\nugetlibs\net9.0\"
}
catch
{
    Write-Host "Copy Error Ignored"
}