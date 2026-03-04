param(
    [switch]$GenerateHtmlReport,
    [switch]$Quiet
)

$repoRoot = Split-Path $PSScriptRoot -Parent
$settingsFile = Join-Path $repoRoot "coverlet.runsettings"
$resultsDir   = Join-Path $repoRoot "TestResults"

# ── Clean previous results ────────────────────────────────────────────────────
if (Test-Path $resultsDir) {
    Remove-Item $resultsDir -Recurse -Force
}

# ── Run tests ────────────────────────────────────────────────────────────────
Write-Host "Running tests..." -ForegroundColor Cyan

$dotnetArgs = @(
    "test", $repoRoot,
    "--settings", $settingsFile,
    "--results-directory", $resultsDir,
    "--nologo"
)

if ($Quiet) {
    $output = & dotnet @dotnetArgs 2>&1
    $exitCode = $LASTEXITCODE

    # Extract pass/fail summary line
    $summary = $output | Where-Object { $_ -match 'passed|failed|skipped' } | Select-Object -Last 1
    if ($summary) { Write-Host $summary }
} else {
    & dotnet @dotnetArgs
    $exitCode = $LASTEXITCODE
}

if ($exitCode -ne 0) {
    Write-Host "Tests failed (exit $exitCode)." -ForegroundColor Red
    exit $exitCode
}

# ── Locate coverage XML ───────────────────────────────────────────────────────
$coverageFile = Get-ChildItem -Path $resultsDir -Filter "coverage.cobertura.xml" -Recurse |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if (-not $coverageFile) {
    Write-Error "No coverage.cobertura.xml found under $resultsDir"
    exit 1
}

Write-Host "Coverage file: $($coverageFile.FullName)" -ForegroundColor DarkGray

# ── Detailed table ────────────────────────────────────────────────────────────
& (Join-Path $PSScriptRoot "DetailedCoverageReport.ps1") -CoverageFile $coverageFile.FullName

# ── Optional HTML report ──────────────────────────────────────────────────────
if ($GenerateHtmlReport) {
    Write-Host "Restoring local dotnet tools..." -ForegroundColor Cyan
    & dotnet tool restore --tool-manifest (Join-Path $repoRoot ".config/dotnet-tools.json")

    $htmlDir = Join-Path $resultsDir "HtmlReport"
    Write-Host "Generating HTML report..." -ForegroundColor Cyan
    & dotnet reportgenerator `
        "-reports:$($coverageFile.FullName)" `
        "-targetdir:$htmlDir" `
        "-reporttypes:Html"

    $indexPath = Join-Path $htmlDir "index.html"
    Write-Host ""
    Write-Host "HTML report: $indexPath" -ForegroundColor Green
}
