@echo off
echo ==========================================
echo   Servisna Kniha - Build
echo ==========================================

where dotnet >nul 2>&1
if errorlevel 1 (
    echo CHYBA: .NET SDK nie je nainstalovane.
    echo Stiahnite z: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo Obnovujem balicky...
dotnet restore ServisnaKniha\ServisnaKniha.csproj

echo Kompilujem...
dotnet build ServisnaKniha\ServisnaKniha.csproj -c Release

if errorlevel 1 (
    echo CHYBA pri kompilacii!
    pause
    exit /b 1
)

echo.
echo ==========================================
echo   Build uspesny!
echo   Spustitelny subor: ServisnaKniha\bin\Release\net8.0-windows\ServisnaKniha.exe
echo ==========================================
pause
