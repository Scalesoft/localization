To generate nuget packages from projects run script BuildNugetPackages.ps1

OR 

use following command:

nuget pack <name of project here>.csproj -IncludeReferencedProjects

This will create nuget package with files and dependencies and info specified in .nuspec file

Project must be built before packaging
