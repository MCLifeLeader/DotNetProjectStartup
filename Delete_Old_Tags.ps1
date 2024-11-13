# Define the time frame (e.g., 60 days ago)
$timeFrame = (Get-Date).AddDays(-60)

# Get all tags and their creation dates
$tags = git for-each-ref --sort=taggerdate --format '%(refname:short) %(taggerdate:iso8601)' refs/tags

# Filter tags older than the specified time frame
$oldTags = $tags | ForEach-Object {
    $tag, $date = $_ -split ' '
    if ([DateTime]::Parse($date) -lt $timeFrame) {
        $tag
    }
}

# Delete old tags locally and remotely
$oldTags | ForEach-Object {
    git tag -d $_
    git push origin --delete $_
}
