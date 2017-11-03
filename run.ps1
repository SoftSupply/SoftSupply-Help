
$IIS_EXE = "${env:ProgramFiles(x86)}\IIS Express\iisexpress.exe"

# Make sure that IIS Express has been installed.
if (!(Test-Path $IIS_EXE)) {
    Throw "Could not find iisexpress.exe at $IIS_EXE"
}

Clear

# Start Site
Write-Host "Running website..."
Invoke-Expression "& `"$IIS_EXE`" /config:`".vs\config\applicationhost.config`" /site:`"SoftSupply-Help`" /apppool:`"Clr4IntegratedAppPool`"" 
exit $LASTEXITCODE