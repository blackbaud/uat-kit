# Install.ps1
param($installPath, $toolsPath, $package, $project)

$xml = New-Object xml

# find the App.config file
$config = $project.ProjectItems | where {$_.Name -eq "App.config"}

# find its path on the file system
$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

# load App.config as XML
$xml.Load($localPath.Value)

# select the node and delete existing if necessary
$node = $xml.SelectSingleNode("//*/specFlow/plugins/add[contains(@name,'Blackbaud.UAT.SpecFlow.Selenium')]")
if ($node)
{
    $node.ParentNode.RemoveChild($node)
}

$pack = $package -replace " ","."

$newnode = $xml.CreateElement("add")
$newnode.SetAttribute("name","Blackbaud.UAT.SpecFlow.Selenium")
$newnode.SetAttribute("path", "..\packages\" + $pack)
# add the node back
$xml.SelectSingleNode("//*/specFlow/plugins").AppendChild($newnode)

# save the App.config file
$xml.Save($localPath.Value)