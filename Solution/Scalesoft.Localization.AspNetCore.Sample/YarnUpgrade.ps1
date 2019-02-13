#!/usr/bin/env pwsh

$CurrentPath = (Get-Location -PSProvider FileSystem).ProviderPath

$FailedCount = 0
$FailedGulpCount = 0
$FailedAuditCount = 0

Write-Host

yarn upgrade --update-checksums --emoji false --no-progress --silent

if ($LASTEXITCODE -ne 0)
{
    $FailedCount++
}

yarn audit

if ($LASTEXITCODE -ne 0)
{
    $FailedAuditCount++
}

Write-Host "1 Yarn projects attempted to upgrade," $FailedCount "failed" -foregroundcolor green
Write-Host "1 Yarn audit executed," $FailedAuditCount "failed" -foregroundcolor green

cd $CurrentPath

Write-Host

if ($FailedCount -gt 0)
{
    exit 1
}
