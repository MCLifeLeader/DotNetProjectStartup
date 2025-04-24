# Define the time frame (e.g., 60 days ago)
$timeFrame = (Get-Date).AddDays(-60)
$pattern = '\d+\.(\d{4})\.(\d{4})\.\d+'

# Get all tags and their creation dates
$tags = git for-each-ref --sort=taggerdate --format '%(refname:short) %(taggerdate:iso8601)' refs/tags

# Write tags to the screen
$oldTags = $tags | ForEach-Object {
    if ($_ -match $pattern) {
        $year = $matches[1]
        $month = $matches[2].Substring(0, 2)
        $day = $matches[2].Substring(2, 2)
        $date = [DateTime]::ParseExact("$year-$month-$day", "yyyy-MM-dd", $null)

        if ($date -lt $timeFrame) {
            $tag, $ignore = $_ -split ' '
            $tag
        }
    }
    else {
        # Remove non-standard tags. All tags should match the pattern "Version.Year.MonthDay.Build", e.g., "1.2024.0101.1"
        $tag, $ignore = $_ -split ' '
        $tag
    }
}

# Delete old tags locally and remotely
$oldTags | ForEach-Object {
    Write-Output "Push Remove $_"
    git tag -d $_
    git push origin --delete $_
}