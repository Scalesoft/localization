#!/usr/bin/env pwsh

    [CmdletBinding()]
Param(
    [Parameter()]
    [Switch]$disableRestore = $false
)

$CurrentPath = (Get-Location -PSProvider FileSystem).ProviderPath

Set-Location Solution

if (!$disableRestore)
{
    dotnet restore
}

$projectsToTest = Get-ChildItem .\ -Include '*.Tests.csproj' -Recurse

foreach ($project in $projectsToTest)
{
    dotnet test --no-restore --filter "TestCategory!=SQL-Server-Required" $project
}

Set-Location $CurrentPath
