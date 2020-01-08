#!/usr/bin/env pwsh

    [CmdletBinding()]
Param(
    [String]$TargetEnvironment = "Production",
    [String]$IisPath = "Default Web Site/TranslatorTool"
)

$env:ASPNETCORE_ENVIRONMENT = "${TargetEnvironment}"
$DotnetVersion = & dotnet --version

$ProjectDir = $PSScriptRoot

Write-Host
Write-Host "Using project directory: ${ProjectDir}"
Write-Host "Using .NET Core SDK ${DotnetVersion}"
Write-Host
Write-Host

$CsprojFile = Join-Path $ProjectDir "Scalesoft.Localization.Tool.Translator.csproj"
$PackageLocation = Join-Path $ProjectDir "bin\Publish-${TargetEnvironment}"
$OutDir = Join-Path $ProjectDir "bin\Publish-build"
$BuildPackageLocation = Join-Path $ProjectDir "bin\temp-package.zip"

dotnet build $CsprojFile --configuration Release /p:PublishProfile=Release /p:EnvironmentName="${TargetEnvironment}" /p:PackageLocation="${PackageLocation}" /p:OutDir="${OutDir}" /p:DeployOnBuild=true /p:WebPublishMethod=Package /p:PackageAsSingleFile=true /maxcpucount:1 /p:platform="Any CPU" /p:configuration="Release" /p:DesktopBuildPackageLocation="${BuildPackageLocation}" /p:DeployIisAppPath="${IisPath}"

Copy-Item "${ProjectDir}\Deploy.Translator.ps1" -Destination "${PackageLocation}"
