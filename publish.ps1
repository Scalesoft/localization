#!/usr/bin/env pwsh

    [CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [String]$apiKey,

    [Parameter(Mandatory = $true)]
    [String]$packageVersion,

    [Parameter()]
    [Switch]$disableRestore = $false
)

$currentPath = (Get-Location -PSProvider FileSystem).ProviderPath

$packages = New-Object System.Collections.ArrayList
$packages.Add("Scalesoft.Localization.AspNetCore.Service") > $null
$packages.Add("Scalesoft.Localization.Core") > $null
$packages.Add("Scalesoft.Localization.Database.Abstractions") > $null
$packages.Add("Scalesoft.Localization.Database.EFCore") > $null
$packages.Add("Scalesoft.Localization.Database.NHibernate") > $null

if ($packageVersion[0] -eq 'v')
{
    $packageVersion = $packageVersion.Substring(1)
}

Set-Location Solution

if (!$disableRestore)
{
    dotnet restore
}

dotnet publish --no-restore "/property:Version=${packageVersion}"

Set-Location $currentPath
Set-Location build

foreach ($package in $packages)
{
    $packageFullName = "${package}.${packageVersion}.nupkg"

    Write-Host "Publishing ${packageFullName}"
    dotnet nuget push --source nuget.org -k "${apiKey}" "${packageFullName}"
}

Set-Location $currentPath
