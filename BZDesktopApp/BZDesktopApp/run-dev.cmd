@echo off
REM Development run script with hot reload
REM This script starts the app with dotnet-watch which enables automatic reload on file changes

echo Starting BZDesktopApp with hot reload enabled...
echo.
echo Changes you make to:
echo  - Razor components (.razor files)
echo  - C# code (.cs files)  
echo  - CSS files
echo will be automatically applied without stopping the app.
echo.
echo Press Ctrl+C to stop the application.
echo.

dotnet watch run

pause
