#!/usr/bin/env pwsh

$CurrentPath = (Get-Location -PSProvider FileSystem).ProviderPath

$FailedCount = 0

Write-Host

yarn outdated

if ($LASTEXITCODE -ne 0)
{
    $FailedCount++
}

Write-Host
Write-Host "1 Yarn projects checked for outdated packages," $FailedCount "failed" -foregroundcolor green

cd $CurrentPath

Write-Host

if ($FailedCount -gt 0)
{
    exit 1
}