@echo off
setlocal
cd /d "%~dp0"

echo ==========================================
echo   Servisna Kniha - Publish (self-contained)
echo ==========================================
echo Adresar: %CD%

where dotnet >nul 2>&1
if errorlevel 1 (
    echo CHYBA: .NET SDK nie je nainstalovane.
    echo Stiahnite z: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

if not exist "ServisnaKniha\ServisnaKniha.csproj" (
    echo CHYBA: Subor ServisnaKniha\ServisnaKniha.csproj nebol najdeny!
    pause
    exit /b 1
)

dotnet publish "ServisnaKniha\ServisnaKniha.csproj" ^
  -c Release ^
  -r win-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -o "%~dp0publish"

if errorlevel 1 (
    echo CHYBA pri publikovani!
    pause
    exit /b 1
)

echo.
echo ==========================================
echo   Publikovanie uspesne!
echo   Subor: %CD%\publish\ServisnaKniha.exe
echo ==========================================
pause
endlocal
