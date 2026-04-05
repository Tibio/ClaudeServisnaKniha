@echo off
setlocal
cd /d "%~dp0"

echo ==========================================
echo   Servisna Kniha - Build + Installer
echo ==========================================

:: ---- 1. Ikona -----------------------------------------------
echo.
echo [1/4] Ikona...

if exist "ServisnaKniha\app.ico" (
    echo      app.ico uz existuje, preskakujem generovanie.
) else (
    powershell -ExecutionPolicy Bypass -File "%~dp0create_icon.ps1"
    if errorlevel 1 (
        echo UPOZORNENIE: Ikonu sa nepodarilo vytvorit. Pokracujem bez nej.
    ) else (
        echo      Ikona vytvorena.
    )
)

:: ---- 2. Dotnet publish ---------------------------------------
echo.
echo [2/4] Kontrola .NET SDK...
where dotnet >nul 2>&1
if errorlevel 1 (
    echo CHYBA: .NET SDK nie je nainstalovane.
    echo Stiahnite z: https://dotnet.microsoft.com/download
    pause & exit /b 1
)

if not exist "ServisnaKniha\ServisnaKniha.csproj" (
    echo CHYBA: Nebol najdeny ServisnaKniha.csproj
    pause & exit /b 1
)

echo.
echo [3/4] Kompilacia a publikovanie (self-contained)...
dotnet publish "ServisnaKniha\ServisnaKniha.csproj" ^
  -c Release ^
  -r win-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -o "%~dp0publish"

if errorlevel 1 (
    echo CHYBA pri kompilacii!
    pause & exit /b 1
)

if not exist "publish\ServisnaKniha.exe" (
    echo CHYBA: publish\ServisnaKniha.exe nebol vytvoreny!
    pause & exit /b 1
)
echo      Aplikacia publikovana: publish\ServisnaKniha.exe

:: ---- 3. Inno Setup ------------------------------------------
echo.
echo [4/4] Vytvaranie instalatora...

set "ISCC="
for %%P in (
    "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    "C:\Program Files\Inno Setup 6\ISCC.exe"
    "C:\Program Files (x86)\Inno Setup 5\ISCC.exe"
    "C:\Program Files\Inno Setup 5\ISCC.exe"
) do (
    if exist %%P set "ISCC=%%P"
)

if "%ISCC%"=="" (
    echo.
    echo UPOZORNENIE: Inno Setup nebol najdeny.
    echo Stiahnite a nainštalujte z: https://jrsoftware.org/isinfo.php
    echo.
    echo Po instalacii spustite znova tento skript, alebo rucne:
    echo   "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer\ServisnaKniha.iss
    echo.
    echo Aplikacia je dostupna priamo: %CD%\publish\ServisnaKniha.exe
    pause & exit /b 0
)

echo      Pouzivam: %ISCC%
%ISCC% "installer\ServisnaKniha.iss"

if errorlevel 1 (
    echo CHYBA pri vytvarani instalatora!
    pause & exit /b 1
)

echo.
echo ==========================================
echo   HOTOVO!
echo.
echo   Instalator: %CD%\installer\ServisnaKniha_Setup_1.0.0.exe
echo   Priama EXE: %CD%\publish\ServisnaKniha.exe
echo ==========================================
pause
endlocal
