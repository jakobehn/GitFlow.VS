#Copy necessary files from the util-linux package into %programfiles(x86)%\Git\bin
#Runs gitflow\contrib\msysgit-install.cmd
#
# NOTE:This script must be executed with elevated priviledges
#

$installationPath = Split-Path $MyInvocation.MyCommand.Path
$targetFolder = $installationPath
$binaries = Join-Path $installationPath "binaries"
$gitFlowFolder = Join-Path $installationPath "gitflow"
$gitLocation = Join-Path ${Env:ProgramFiles(x86)} "Git\bin"

Write-Host "Copy binaries to Git installation directory"
Copy-Item -Path "$binaries\*.*" -Destination "$gitLocation" -Force

#Check if gitflow need to be installed
if(Test-Path (Join-Path $gitLocation "git-flow"))
{
    Write-Host "GitFlow already installed"
    exit
}

#Run gitflow install script
$installScript = Join-Path $installationPath "gitflow\contrib\msysgit-install.cmd"
$pinfo = New-Object System.Diagnostics.ProcessStartInfo
$pinfo.FileName = $installScript
$pinfo.UseShellExecute = $true
$pinfo.Arguments = """c:\program files (x86)\git"""

$p = New-Object System.Diagnostics.Process
$p.StartInfo = $pinfo
$p.Start() | Out-Null
$p.WaitForExit() 

Write-Host "Installation done!"



