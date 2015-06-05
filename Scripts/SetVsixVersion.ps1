param(
    [string]$buildVersion
    )

$paramFiles=get-childitem "*.vsixmanifest" -rec -Verbose
foreach ($file in $paramFiles)
{
    Write-Host -Verbose ("Replacing version in " + $file.PSPath + " with version " + $buildVersion)
	(Get-Content $file.PSPath) | Foreach-Object { $_ -replace "1.0.0", "$buildVersion" } | Set-Content $file.PSPath
}