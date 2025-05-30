# Get the list of all running jobs.
Get-Job

# To filter, first you need to assign the currently running jobs to a variable like $jobs.
$jobs = Get-Job

# Search the list of jobs for the one you want to stop.
$job = $jobs | Where-Object { $_.Command -like '*Api.Startup.Example*' }

# Use either Stop-Job or Remove-Job to kill the job.
Stop-Job $job.Id
Remove-Job $job.Id