#Copy necessary files from the util-linux package into the bin directory of the msysgit installation
#Runs gitflow\contrib\msysgit-install.cmd
#
# NOTE:This script must be executed with elevated priviledges, in case Git is installed below program files
#
Param
(
	[Parameter(Mandatory=$True)] [string] $gitInstallPath
)
Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi
az extension add --name azure-devops
$installationPath = Split-Path $MyInvocation.MyCommand.Path
$targetFolder = $installationPath
$binaries = Join-Path $installationPath "binaries"
$gitFlowFolder = Join-Path $installationPath "gitflow"
$gitLocation = Join-Path $gitInstallPath "bin"

Write-Host "Copy binaries to Git installation directory " + $gitLocation

Copy-Item -Path "$binaries\*.*" -Destination "$gitLocation" -Force -Verbose

#Check if gitflow need to be installed
#if(Test-Path (Join-Path $gitLocation "git-flow"))
#{
#    Write-Host "GitFlow already installed"
#    exit
#}

#Run gitflow install script
$installScript = Join-Path $installationPath "gitflow\contrib\msysgit-install.cmd"
$pinfo = New-Object System.Diagnostics.ProcessStartInfo
$pinfo.FileName = $installScript
$pinfo.UseShellExecute = $true
$pinfo.Arguments = """$gitInstallPath"""

$p = New-Object System.Diagnostics.Process
$p.StartInfo = $pinfo
$p.Start() | Out-Null
$p.WaitForExit() 

Write-Host "Installation done!"



