echo off

set KSPDIR=..\KSP

set KSPLIB=%KSPDIR%\KSP_DATA\Managed
set KSPPLUGINS=%KSPDIR%\Plugins
set KSPPARTS=%KSPDIR%\Parts

set PATH=%PATH%;%systemroot%\Microsoft.NET\Framework\v4.0.30319

robocopy /MIR Parts F13Aerospace\Parts
robocopy /MIR Parts\F13SolarPanel "%KSPPARTS%\F13SolarPanel"
copy README F13Aerospace
mkdir F13Aerospace\Plugins

csc "/r:%KSPLIB%\UnityEngine.dll" "/r:%KSPLIB%\Assembly-CSharp.dll" /t:library /out:F13Aerospace\Plugins\F13AerospacePlugin.dll F13SolarPanel.cs F13Utils.cs
if not %errorlevel%==0 goto fail

copy F13Aerospace\Plugins\F13AerospacePlugin.dll "%KSPPLUGINS%\"
if not %errorlevel%==0 goto fail

echo --- SUCCESS ---
exit /b 0

:fail
echo --- BUILD FAILURE ---
pause
