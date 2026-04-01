# Servisná kniha vozidiel

Elektronická servisná kniha pre autá — Windows desktopová aplikácia v C# WinForms s SQLite databázou.

## Funkcie

| Modul | Popis |
|-------|-------|
| **Vozidlá** | Evidencia áut (ŠPZ, VIN, motor, farba, STK/EK termíny) |
| **Servisné záznamy** | História servisov s cenami práce a dielov |
| **OEM diely** | Evidencia vymenených dielov s OEM číslami |
| **Náklady** | Prehľad všetkých výdavkov podľa kategórií |
| **Termíny servisu** | Pripomienky dátumom aj km s opakovaním |
| **Sledovanie km** | História stavu kilometrov |

## Požiadavky

- Windows 10 / 11
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (pre kompiláciu)

## Zostavenie

```bat
build.bat
```

Výsledok: `ServisnaKniha\bin\Release\net8.0-windows\ServisnaKniha.exe`

### Publish ako jeden .exe súbor

```bat
build_publish.bat
```

Výsledok: `publish\ServisnaKniha.exe` (self-contained, nevyžaduje nainštalovaný .NET)

## Databáza

SQLite databáza sa ukladá do:
```
%LOCALAPPDATA%\ServisnaKniha\servisna_kniha.db
```

## Štruktúra projektu

```
ServisnaKniha/
├── Program.cs                     # Vstupný bod aplikácie
├── GlobalUsings.cs                # Globálne using direktívy
├── ServisnaKniha.csproj           # Projektový súbor (.NET 8 WinForms)
├── Models/
│   ├── Auto.cs                    # Model vozidla
│   ├── ServisnyZaznam.cs          # Model servisného záznamu
│   ├── OEMDiel.cs                 # Model OEM dielu
│   ├── Naklad.cs                  # Model nákladu
│   ├── ServisnyTermin.cs          # Model servisného termínu
│   └── KMZaznam.cs                # Model km záznamu
├── Database/
│   └── DatabaseManager.cs         # SQLite dátová vrstva (CRUD)
└── Forms/
    ├── HlavnyFormular.cs          # Hlavné okno s TabControl
    ├── AutoDialog.cs              # Dialóg pridania/úpravy vozidla
    ├── ServisnyZaznamDialog.cs    # Dialóg servisného záznamu
    ├── OEMDielDialog.cs           # Dialóg OEM dielu
    ├── NakladDialog.cs            # Dialóg nákladu
    ├── TerminDialog.cs            # Dialóg servisného termínu
    └── KMSledovaniePanel.cs       # Panel sledovania km
```

## Farebné označenie

- **Červené** riadky — prekoročená STK/EK alebo expirovaný termín
- **Žlté** riadky — blížiaca sa STK/EK alebo termín (do 30 dní / 500 km)
- **Zelené** riadky — v poriadku