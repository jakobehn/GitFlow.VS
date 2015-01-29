$installationPath = Split-Path $MyInvocation.MyCommand.Path
$sourceFile = Join-Path  $installationPath "Dependencies.zip"
$targetFolder = $installationPath
$binaries = Join-Path $installationPath "binaries"
$gitFlowFolder = Join-Path $installationPath "gitflow"

if (Test-Path $binaries){
    Write-Host "Removing binaries directory"
	Remove-Item $binaries -Recurse
}

if (Test-Path $gitFlowFolder){
    Write-Host "Removing GitFlow directory"
	Remove-Item $gitFlowFolder -Recurse
}

[System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem')
[System.IO.Compression.ZipFile]::ExtractToDirectory($sourceFile, $targetFolder)

Write-Host "Extracting files"
$gitLocation = Join-Path ${Env:ProgramFiles(x86)} "Git\bin"
Sleep 1

Write-Host "Copy binaries to Git installation directory"
Copy-Item -Path "$binaries\*.*" -Destination "$gitLocation" -Force

if(Test-Path (Join-Path $gitLocation "git-flow"))
{
    Write-Host "GitFlow already installed"
    exit
}
Write-Host "Installing gitflow"
$installScript = Join-Path $installationPath "gitflow\contrib\msysgit-install.cmd"

#Start-Process -FilePath "$installScript" -Wait -passthru
$pinfo = New-Object System.Diagnostics.ProcessStartInfo
$pinfo.FileName = $installScript
$pinfo.UseShellExecute = $true

$pinfo.Arguments = """c:\program files (x86)\git"""

$p = New-Object System.Diagnostics.Process
$p.StartInfo = $pinfo
$p.Start() | Out-Null
$p.WaitForExit() 
Write-Host "exit code: " + $p.ExitCode

Write-Host "Installation done!"
Sleep 1


