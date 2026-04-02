; =========================================================
;  Servisna Kniha Vozidiel — Inno Setup 6 installer script
; =========================================================

#define AppName      "Servisna Kniha"
#define AppVersion   "1.0.0"
#define AppPublisher "ServisnaKniha"
#define AppURL       "https://github.com/Tibio/ClaudeServisnaKniha"
#define AppExeName   "ServisnaKniha.exe"
#define AppDataDir   "{localappdata}\ServisnaKniha"
#define SourceDir    "..\publish"
#define IconFile     "..\ServisnaKniha\app.ico"

[Setup]
AppId={{A7B3C2D1-E4F5-4A6B-8C9D-0E1F2A3B4C5D}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
AllowNoIcons=yes
LicenseFile=
PrivilegesRequired=admin
OutputDir=.
OutputBaseFilename=ServisnaKniha_Setup_{#AppVersion}
SetupIconFile={#IconFile}
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
WizardSizePercent=120
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\{#AppExeName}
UninstallDisplayName={#AppName}
VersionInfoVersion={#AppVersion}
VersionInfoCompany={#AppPublisher}
VersionInfoDescription={#AppName} — Elektronicka servisna kniha pre auta
VersionInfoProductName={#AppName}

; Minimalny Windows 10
MinVersion=10.0

[Languages]
Name: "slovak";  MessagesFile: "compiler:Languages\Slovak.isl";  \
    LicenseFile: ""; InfoBeforeFile: ""; InfoAfterFile: ""
Name: "english"; MessagesFile: "compiler:Default.isl"

[CustomMessages]
slovak.LaunchApp=Spustit {#AppName} po dokonceni instalacie
english.LaunchApp=Launch {#AppName} after installation

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; \
    GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; \
    GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1

[Files]
Source: "{#SourceDir}\{#AppExeName}"; DestDir: "{app}"; Flags: ignoreversion
; Volitelne — ak existuju extra subory v publish adresari
Source: "{#SourceDir}\*.dll";  DestDir: "{app}"; Flags: ignoreversion recursesubdirs skipifsourcedoesntexist
Source: "{#SourceDir}\*.json"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs skipifsourcedoesntexist

[Icons]
Name: "{group}\{#AppName}";         Filename: "{app}\{#AppExeName}"; \
    IconFilename: "{app}\{#AppExeName}"
Name: "{group}\Odinstalovar {#AppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppName}";   Filename: "{app}\{#AppExeName}"; \
    Tasks: desktopicon; IconFilename: "{app}\{#AppExeName}"

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchApp}"; \
    Flags: nowait postinstall skipifsilent

[UninstallRun]
; Aplikacia si data drzi v %LOCALAPPDATA%, pri odinstalacii sa nemazuzu
; (zachovanie zalohy databazy). Odkomentuj nasledujuci riadok ak chces mazat data:
; Filename: "cmd.exe"; Parameters: "/C rmdir /S /Q ""{#AppDataDir}"""; Flags: runhidden

[Code]
// Kontrola ci uz je aplikacia spustena pred instalovou
function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
begin
  Result := True;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then begin
    // Nic specialne
  end;
end;
