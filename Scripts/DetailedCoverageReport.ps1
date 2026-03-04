param(
    [Parameter(Mandatory = $true)]
    [string]$CoverageFile
)

if (-not (Test-Path $CoverageFile)) {
    Write-Error "Coverage file not found: $CoverageFile"
    exit 1
}

[xml]$xml = Get-Content $CoverageFile

$rows = @()
$totalStmts = 0
$totalMiss = 0
$totalBranches = 0
$totalBranchMiss = 0

foreach ($package in $xml.coverage.packages.package) {
    foreach ($class in $package.classes.class) {
        $filename = $class.filename

        # Skip test/obj paths
        if ($filename -match '\\obj\\' -or $filename -match '/obj/' -or
            $filename -match '\.Tests\\' -or $filename -match '\.Tests/') {
            continue
        }

        # Derive display name from filename (relative, forward slashes)
        $displayName = $filename -replace '\\', '/'

        # Statements = total lines; Miss = uncovered lines
        $lines = $class.lines.line
        $stmts = if ($lines) { @($lines).Count } else { 0 }
        $miss = if ($lines) { @($lines | Where-Object { $_.hits -eq '0' }).Count } else { 0 }

        # Branches — parse condition-coverage="50% (1/2)" from branch lines
        $totalBranchesInClass = 0
        $coveredBranchesInClass = 0
        foreach ($line in @($lines)) {
            $cc = $line.'condition-coverage'
            if ($cc -and $cc -match '\((\d+)/(\d+)\)') {
                $coveredBranchesInClass += [int]$Matches[1]
                $totalBranchesInClass   += [int]$Matches[2]
            }
        }
        $branchMiss = $totalBranchesInClass - $coveredBranchesInClass

        # Missing line numbers
        $missingLines = @()
        if ($lines) {
            $uncoveredNums = @($lines | Where-Object { $_.hits -eq '0' } | ForEach-Object { [int]$_.number }) | Sort-Object
            # Collapse consecutive ranges
            if ($uncoveredNums.Count -gt 0) {
                $rangeStart = $uncoveredNums[0]
                $rangeEnd = $uncoveredNums[0]
                for ($i = 1; $i -lt $uncoveredNums.Count; $i++) {
                    if ($uncoveredNums[$i] -eq $rangeEnd + 1) {
                        $rangeEnd = $uncoveredNums[$i]
                    } else {
                        $missingLines += if ($rangeStart -eq $rangeEnd) { "$rangeStart" } else { "$rangeStart-$rangeEnd" }
                        $rangeStart = $uncoveredNums[$i]
                        $rangeEnd = $uncoveredNums[$i]
                    }
                }
                $missingLines += if ($rangeStart -eq $rangeEnd) { "$rangeStart" } else { "$rangeStart-$rangeEnd" }
            }
        }

        # Cover%
        $cover = if ($stmts -gt 0) { [math]::Round(($stmts - $miss) / $stmts * 100) } else { 100 }

        $totalStmts += $stmts
        $totalMiss += $miss
        $totalBranches += $totalBranchesInClass
        $totalBranchMiss += $branchMiss

        $rows += [PSCustomObject]@{
            Name    = $displayName
            Stmts   = $stmts
            Miss    = $miss
            Branch  = $totalBranchesInClass
            BrPart  = $branchMiss
            Cover   = $cover
            Missing = ($missingLines -join ', ')
        }
    }
}

if ($rows.Count -eq 0) {
    Write-Host "No coverage data found (all files may be excluded)."
    exit 0
}

# Column widths
$nameWidth   = [math]::Max(4, ($rows | ForEach-Object { $_.Name.Length } | Measure-Object -Maximum).Maximum)
$stmtsWidth  = [math]::Max(5, 5)
$missWidth   = [math]::Max(4, 4)
$branchWidth = [math]::Max(6, 6)
$brPartWidth = [math]::Max(6, 6)
$coverWidth  = [math]::Max(5, 5)

$totalCoverPct = if ($totalStmts -gt 0) { [math]::Round(($totalStmts - $totalMiss) / $totalStmts * 100) } else { 100 }

function Format-Row($name, $stmts, $miss, $branch, $brPart, $cover, $missing) {
    $coverStr = "${cover}%"
    "{0,-$nameWidth}  {1,$stmtsWidth}  {2,$missWidth}  {3,$branchWidth}  {4,$brPartWidth}  {5,$coverWidth}  {6}" -f `
        $name, $stmts, $miss, $branch, $brPart, $coverStr, $missing
}

$separator = ("-" * $nameWidth) + "  " + ("-" * $stmtsWidth) + "  " + ("-" * $missWidth) + "  " +
             ("-" * $branchWidth) + "  " + ("-" * $brPartWidth) + "  " + ("-" * $coverWidth) + "  " + ("-" * 7)

Write-Host ""
Write-Host (Format-Row "Name" "Stmts" "Miss" "Branch" "BrPart" "Cover" "Missing")
Write-Host $separator
foreach ($row in ($rows | Sort-Object Name)) {
    Write-Host (Format-Row $row.Name $row.Stmts $row.Miss $row.Branch $row.BrPart $row.Cover $row.Missing)
}
Write-Host $separator
Write-Host (Format-Row "TOTAL" $totalStmts $totalMiss $totalBranches $totalBranchMiss $totalCoverPct "")
Write-Host ""
