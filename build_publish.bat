@echo off
echo ==========================================
echo   Servisna Kniha - Publish (self-contained)
echo ==========================================

dotnet publish ServisnaKniha\ServisnaKniha.csproj ^
  -c Release ^
  -r win-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -o publish\

if errorlevel 1 (
    echo CHYBA pri publikovani!
    pause
    exit /b 1
)

echo.
echo ==========================================
echo   Publikovanie uspesne!
echo   Subor: publish\ServisnaKniha.exe
echo ==========================================
pause
