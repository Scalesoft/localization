param($installPath, $toolsPath, $package, $project)

$configItem = $project.ProjectItems.Item("Localization.Container.config")

# set 'Copy To Output Directory' to 'Copy always'
$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")

# Copy Always Always copyToOutput.Value = 1
# Copy if Newer copyToOutput.Value = 2  
$copyToOutput.Value = 1

# set 'Build Action' to 'Content'
$buildAction = $configItem.Properties.Item("BuildAction")
$buildAction.Value = 2