#!/usr/bin/env pwsh

$CurrentPath = (Get-Location -PSProvider FileSystem).ProviderPath

$FailedCount = 0

Write-Host

yarn install

if ($LASTEXITCODE -ne 0)
{
    $FailedCount++
}

yarn audit

Write-Host

$MessageColor = "green"
if ($FailedCount -gt 0)
{
    $MessageColor = "red"
}
Write-Host "1 Yarn projects attempted to install," $FailedCount "failed" -foregroundcolor $MessageColor

cd $CurrentPath

Write-Host

if ($FailedCount -gt 0)
{
    exit 1
}
