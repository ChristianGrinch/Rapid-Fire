@echo off
REM Extract version number from ProjectSettings.asset
for /f "tokens=2 delims=: " %%a in ('findstr /c:"bundleVersion:" "C:\Users\chris\Unity Projects\Terra Decay\ProjectSettings\ProjectSettings.asset"') do set version=%%a

REM Set the path to the builds folder using the version number
set buildPath=C:\Users\chris\Unity Projects\Built Projects\%version%

REM Push the build to itch.io using Butler
butler push "%buildPath%" christiangrinch/terra-decay:windows --userversion %version%

pause
