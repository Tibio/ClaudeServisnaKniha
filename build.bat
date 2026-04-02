@echo off
setlocal
cd /d "%~dp0"

echo ==========================================
echo   Servisna Kniha - Build
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
    echo Uistite sa, ze spustate build.bat z priecinka repozitara.
    pause
    exit /b 1
)

echo Obnovujem balicky...
dotnet restore "ServisnaKniha\ServisnaKniha.csproj"
if errorlevel 1 ( echo CHYBA pri obnove balickov! & pause & exit /b 1 )

echo Kompilujem...
dotnet build "ServisnaKniha\ServisnaKniha.csproj" -c Release
if errorlevel 1 ( echo CHYBA pri kompilacii! & pause & exit /b 1 )

echo.
echo ==========================================
echo   Build uspesny!
echo   Spustitelny subor:
echo   %CD%\ServisnaKniha\bin\Release\net8.0-windows\ServisnaKniha.exe
echo ==========================================
pause
endlocal
