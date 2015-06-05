
$paramFiles=get-childitem "*.vsixmanifest" -rec -Verbose
foreach ($file in $paramFiles)
{
    Write-Host -Verbose ("Replacing version in " + $file.PSPath + " with version " + $env:BUILD_BUILDNUMBER)
	(Get-Content $file.PSPath) | Foreach-Object { $_ -replace "1.0.0", "$env:BUILD_BUILDNUMBER" } | Set-Content $file.PSPath
}