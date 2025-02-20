#!/usr/bin/env pwsh

$CurrentPath = (Get-Location -PSProvider FileSystem).ProviderPath

$FailedCount = 0

Write-Host

npm install

if ($LASTEXITCODE -ne 0)
{
    $FailedCount++
}

npm audit --registry=https://registry.npmjs.org

Write-Host

$MessageColor = "green"
if ($FailedCount -gt 0)
{
    $MessageColor = "red"
}
Write-Host "1 NPM projects attempted to install," $FailedCount "failed" -foregroundcolor $MessageColor

cd $CurrentPath

Write-Host

if ($FailedCount -gt 0)
{
    exit 1
}
