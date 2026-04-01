using Microsoft.Data.Sqlite;
using ServisnaKniha.Models;

namespace ServisnaKniha.Database;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    private SqliteConnection GetConnection() => new(_connectionString);

    private void InitializeDatabase()
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            PRAGMA journal_mode=WAL;
            PRAGMA foreign_keys=ON;

            CREATE TABLE IF NOT EXISTS Auto (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SPZ TEXT NOT NULL,
                Znacka TEXT NOT NULL,
                Model TEXT NOT NULL,
                RokVyroby INTEGER NOT NULL,
                VIN TEXT,
                TypMotora TEXT,
                ObjemMotora REAL,
                VykonKW INTEGER,
                Farba TEXT,
                AktualneKM INTEGER DEFAULT 0,
                DatumSTK TEXT,
                DatumEK TEXT,
                Poznamka TEXT,
                DatumPridania TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ServisnyZaznam (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AutoId INTEGER NOT NULL,
                DatumServisu TEXT NOT NULL,
                StavKM INTEGER NOT NULL,
                TypServisu TEXT NOT NULL,
                Popis TEXT,
                Servis TEXT,
                CelkovaCena REAL DEFAULT 0,
                CenaPrace REAL DEFAULT 0,
                CenaDielov REAL DEFAULT 0,
                Poznamka TEXT,
                DatumPridania TEXT NOT NULL,
                FOREIGN KEY (AutoId) REFERENCES Auto(Id) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS OEMDiel (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AutoId INTEGER NOT NULL,
                ServisnyZaznamId INTEGER,
                NazovDielu TEXT NOT NULL,
                OEMCislo TEXT,
                Vyrobca TEXT,
                Kategoria TEXT,
                Pocet INTEGER DEFAULT 1,
                CenaKus REAL DEFAULT 0,
                DatumMontaze TEXT,
                KMPriMontazi INTEGER DEFAULT 0,
                Poznamka TEXT,
                FOREIGN KEY (AutoId) REFERENCES Auto(Id) ON DELETE CASCADE,
                FOREIGN KEY (ServisnyZaznamId) REFERENCES ServisnyZaznam(Id) ON DELETE SET NULL
            );

            CREATE TABLE IF NOT EXISTS Naklad (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AutoId INTEGER NOT NULL,
                ServisnyZaznamId INTEGER,
                Datum TEXT NOT NULL,
                Kategoria TEXT NOT NULL,
                Popis TEXT,
                Suma REAL NOT NULL,
                Mena TEXT DEFAULT 'EUR',
                Doklad TEXT,
                Poznamka TEXT,
                FOREIGN KEY (AutoId) REFERENCES Auto(Id) ON DELETE CASCADE,
                FOREIGN KEY (ServisnyZaznamId) REFERENCES ServisnyZaznam(Id) ON DELETE SET NULL
            );

            CREATE TABLE IF NOT EXISTS ServisnyTermin (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AutoId INTEGER NOT NULL,
                Nazov TEXT NOT NULL,
                Popis TEXT,
                DatumTerminu TEXT,
                KMTerminu INTEGER,
                OpakovanieDni INTEGER,
                OpakovaniKM INTEGER,
                JeAktivny INTEGER DEFAULT 1,
                PosledneVykonanie TEXT,
                KMPoslednehoVykonania INTEGER,
                Poznamka TEXT,
                FOREIGN KEY (AutoId) REFERENCES Auto(Id) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS KMZaznam (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AutoId INTEGER NOT NULL,
                Datum TEXT NOT NULL,
                StavKM INTEGER NOT NULL,
                Poznamka TEXT,
                FOREIGN KEY (AutoId) REFERENCES Auto(Id) ON DELETE CASCADE
            );
        ";
        cmd.ExecuteNonQuery();
    }

    // ==================== AUTO ====================

    public List<Auto> GetVsetkyAuta()
    {
        var list = new List<Auto>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Auto ORDER BY Znacka, Model";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(MapAuto(reader));
        return list;
    }

    public Auto? GetAuto(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Auto WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        return reader.Read() ? MapAuto(reader) : null;
    }

    public int PridajAuto(Auto auto)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Auto (SPZ, Znacka, Model, RokVyroby, VIN, TypMotora, ObjemMotora, VykonKW, Farba, AktualneKM, DatumSTK, DatumEK, Poznamka, DatumPridania)
            VALUES ($spz, $znacka, $model, $rok, $vin, $typ, $objem, $vykon, $farba, $km, $stk, $ek, $pozn, $datum);
            SELECT last_insert_rowid();";
        SetAutoParams(cmd, auto);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void AktualizujAuto(Auto auto)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Auto SET SPZ=$spz, Znacka=$znacka, Model=$model, RokVyroby=$rok, VIN=$vin,
            TypMotora=$typ, ObjemMotora=$objem, VykonKW=$vykon, Farba=$farba, AktualneKM=$km,
            DatumSTK=$stk, DatumEK=$ek, Poznamka=$pozn WHERE Id=$id";
        SetAutoParams(cmd, auto);
        cmd.Parameters.AddWithValue("$id", auto.Id);
        cmd.ExecuteNonQuery();
    }

    public void VymazAuto(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "PRAGMA foreign_keys=ON; DELETE FROM Auto WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private static void SetAutoParams(SqliteCommand cmd, Auto auto)
    {
        cmd.Parameters.AddWithValue("$spz", auto.SPZ);
        cmd.Parameters.AddWithValue("$znacka", auto.Znacka);
        cmd.Parameters.AddWithValue("$model", auto.Model);
        cmd.Parameters.AddWithValue("$rok", auto.RokVyroby);
        cmd.Parameters.AddWithValue("$vin", (object?)auto.VIN ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$typ", (object?)auto.TypMotora ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$objem", auto.ObjemMotora);
        cmd.Parameters.AddWithValue("$vykon", auto.VykonKW);
        cmd.Parameters.AddWithValue("$farba", (object?)auto.Farba ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$km", auto.AktualneKM);
        cmd.Parameters.AddWithValue("$stk", auto.DatumSTK.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$ek", auto.DatumEK.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$pozn", (object?)auto.Poznamka ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$datum", auto.DatumPridania.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    private static Auto MapAuto(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        SPZ = r.GetString(1),
        Znacka = r.GetString(2),
        Model = r.GetString(3),
        RokVyroby = r.GetInt32(4),
        VIN = r.IsDBNull(5) ? "" : r.GetString(5),
        TypMotora = r.IsDBNull(6) ? "" : r.GetString(6),
        ObjemMotora = r.IsDBNull(7) ? 0 : r.GetDouble(7),
        VykonKW = r.IsDBNull(8) ? 0 : r.GetInt32(8),
        Farba = r.IsDBNull(9) ? "" : r.GetString(9),
        AktualneKM = r.IsDBNull(10) ? 0 : r.GetInt32(10),
        DatumSTK = r.IsDBNull(11) ? DateTime.Today : DateTime.Parse(r.GetString(11)),
        DatumEK = r.IsDBNull(12) ? DateTime.Today : DateTime.Parse(r.GetString(12)),
        Poznamka = r.IsDBNull(13) ? "" : r.GetString(13),
        DatumPridania = DateTime.Parse(r.GetString(14))
    };

    // ==================== SERVISNY ZAZNAM ====================

    public List<ServisnyZaznam> GetServisneZaznamy(int? autoId = null)
    {
        var list = new List<ServisnyZaznam>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT s.*, a.Znacka || ' ' || a.Model || ' (' || a.SPZ || ')' as AutoNazov
            FROM ServisnyZaznam s JOIN Auto a ON s.AutoId=a.Id
            " + (autoId.HasValue ? "WHERE s.AutoId=$autoId " : "") +
            "ORDER BY s.DatumServisu DESC";
        if (autoId.HasValue) cmd.Parameters.AddWithValue("$autoId", autoId.Value);
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) list.Add(MapServisnyZaznam(reader));
        return list;
    }

    public int PridajServisnyZaznam(ServisnyZaznam z)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO ServisnyZaznam (AutoId, DatumServisu, StavKM, TypServisu, Popis, Servis, CelkovaCena, CenaPrace, CenaDielov, Poznamka, DatumPridania)
            VALUES ($aid, $datum, $km, $typ, $popis, $servis, $celk, $prace, $diely, $pozn, $pridanie);
            SELECT last_insert_rowid();";
        SetZaznamParams(cmd, z);
        var id = Convert.ToInt32(cmd.ExecuteScalar());
        AktualizujKMAuta(z.AutoId, z.StavKM);
        return id;
    }

    public void AktualizujServisnyZaznam(ServisnyZaznam z)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE ServisnyZaznam SET AutoId=$aid, DatumServisu=$datum, StavKM=$km, TypServisu=$typ,
            Popis=$popis, Servis=$servis, CelkovaCena=$celk, CenaPrace=$prace, CenaDielov=$diely,
            Poznamka=$pozn WHERE Id=$id";
        SetZaznamParams(cmd, z);
        cmd.Parameters.AddWithValue("$id", z.Id);
        cmd.ExecuteNonQuery();
    }

    public void VymazServisnyZaznam(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM ServisnyZaznam WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private static void SetZaznamParams(SqliteCommand cmd, ServisnyZaznam z)
    {
        cmd.Parameters.AddWithValue("$aid", z.AutoId);
        cmd.Parameters.AddWithValue("$datum", z.DatumServisu.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$km", z.StavKM);
        cmd.Parameters.AddWithValue("$typ", z.TypServisu);
        cmd.Parameters.AddWithValue("$popis", (object?)z.Popis ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$servis", (object?)z.Servis ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$celk", z.CelkovaCena);
        cmd.Parameters.AddWithValue("$prace", z.CenaPrace);
        cmd.Parameters.AddWithValue("$diely", z.CenaDielov);
        cmd.Parameters.AddWithValue("$pozn", (object?)z.Poznamka ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$pridanie", z.DatumPridania.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    private static ServisnyZaznam MapServisnyZaznam(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        AutoId = r.GetInt32(1),
        DatumServisu = DateTime.Parse(r.GetString(2)),
        StavKM = r.GetInt32(3),
        TypServisu = r.GetString(4),
        Popis = r.IsDBNull(5) ? "" : r.GetString(5),
        Servis = r.IsDBNull(6) ? "" : r.GetString(6),
        CelkovaCena = r.IsDBNull(7) ? 0 : (decimal)r.GetDouble(7),
        CenaPrace = r.IsDBNull(8) ? 0 : (decimal)r.GetDouble(8),
        CenaDielov = r.IsDBNull(9) ? 0 : (decimal)r.GetDouble(9),
        Poznamka = r.IsDBNull(10) ? "" : r.GetString(10),
        DatumPridania = DateTime.Parse(r.GetString(11)),
        AutoNazov = r.GetString(12)
    };

    // ==================== OEM DIELY ====================

    public List<OEMDiel> GetOEMDiely(int? autoId = null, int? zaznamId = null)
    {
        var list = new List<OEMDiel>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        var where = new List<string>();
        if (autoId.HasValue) where.Add("d.AutoId=$autoId");
        if (zaznamId.HasValue) where.Add("d.ServisnyZaznamId=$zaznamId");
        cmd.CommandText = @"
            SELECT d.*, a.Znacka || ' ' || a.Model || ' (' || a.SPZ || ')' as AutoNazov
            FROM OEMDiel d JOIN Auto a ON d.AutoId=a.Id
            " + (where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "") +
            " ORDER BY d.DatumMontaze DESC";
        if (autoId.HasValue) cmd.Parameters.AddWithValue("$autoId", autoId.Value);
        if (zaznamId.HasValue) cmd.Parameters.AddWithValue("$zaznamId", zaznamId.Value);
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) list.Add(MapOEMDiel(reader));
        return list;
    }

    public int PridajOEMDiel(OEMDiel d)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO OEMDiel (AutoId, ServisnyZaznamId, NazovDielu, OEMCislo, Vyrobca, Kategoria, Pocet, CenaKus, DatumMontaze, KMPriMontazi, Poznamka)
            VALUES ($aid, $zid, $nazov, $oem, $vyrobca, $kat, $pocet, $cena, $datum, $km, $pozn);
            SELECT last_insert_rowid();";
        SetDielParams(cmd, d);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void AktualizujOEMDiel(OEMDiel d)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE OEMDiel SET AutoId=$aid, ServisnyZaznamId=$zid, NazovDielu=$nazov, OEMCislo=$oem,
            Vyrobca=$vyrobca, Kategoria=$kat, Pocet=$pocet, CenaKus=$cena, DatumMontaze=$datum,
            KMPriMontazi=$km, Poznamka=$pozn WHERE Id=$id";
        SetDielParams(cmd, d);
        cmd.Parameters.AddWithValue("$id", d.Id);
        cmd.ExecuteNonQuery();
    }

    public void VymazOEMDiel(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM OEMDiel WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private static void SetDielParams(SqliteCommand cmd, OEMDiel d)
    {
        cmd.Parameters.AddWithValue("$aid", d.AutoId);
        cmd.Parameters.AddWithValue("$zid", (object?)d.ServisnyZaznamId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$nazov", d.NazovDielu);
        cmd.Parameters.AddWithValue("$oem", (object?)d.OEMCislo ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$vyrobca", (object?)d.Vyrobca ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$kat", (object?)d.Kategoria ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$pocet", d.Pocet);
        cmd.Parameters.AddWithValue("$cena", d.CenaKus);
        cmd.Parameters.AddWithValue("$datum", d.DatumMontaze.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$km", d.KMPriMontazi);
        cmd.Parameters.AddWithValue("$pozn", (object?)d.Poznamka ?? DBNull.Value);
    }

    private static OEMDiel MapOEMDiel(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        AutoId = r.GetInt32(1),
        ServisnyZaznamId = r.IsDBNull(2) ? null : r.GetInt32(2),
        NazovDielu = r.GetString(3),
        OEMCislo = r.IsDBNull(4) ? "" : r.GetString(4),
        Vyrobca = r.IsDBNull(5) ? "" : r.GetString(5),
        Kategoria = r.IsDBNull(6) ? "" : r.GetString(6),
        Pocet = r.GetInt32(7),
        CenaKus = (decimal)r.GetDouble(8),
        DatumMontaze = r.IsDBNull(9) ? DateTime.Today : DateTime.Parse(r.GetString(9)),
        KMPriMontazi = r.IsDBNull(10) ? 0 : r.GetInt32(10),
        Poznamka = r.IsDBNull(11) ? "" : r.GetString(11),
        AutoNazov = r.GetString(12)
    };

    // ==================== NAKLADY ====================

    public List<Naklad> GetNaklady(int? autoId = null)
    {
        var list = new List<Naklad>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT n.*, a.Znacka || ' ' || a.Model || ' (' || a.SPZ || ')' as AutoNazov
            FROM Naklad n JOIN Auto a ON n.AutoId=a.Id
            " + (autoId.HasValue ? "WHERE n.AutoId=$autoId " : "") +
            "ORDER BY n.Datum DESC";
        if (autoId.HasValue) cmd.Parameters.AddWithValue("$autoId", autoId.Value);
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) list.Add(MapNaklad(reader));
        return list;
    }

    public decimal GetCelkoveNaklady(int autoId)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COALESCE(SUM(Suma), 0) FROM Naklad WHERE AutoId=$id";
        cmd.Parameters.AddWithValue("$id", autoId);
        return (decimal)(double)(cmd.ExecuteScalar() ?? 0.0);
    }

    public int PridajNaklad(Naklad n)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Naklad (AutoId, ServisnyZaznamId, Datum, Kategoria, Popis, Suma, Mena, Doklad, Poznamka)
            VALUES ($aid, $zid, $datum, $kat, $popis, $suma, $mena, $doklad, $pozn);
            SELECT last_insert_rowid();";
        SetNakladParams(cmd, n);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void AktualizujNaklad(Naklad n)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Naklad SET AutoId=$aid, ServisnyZaznamId=$zid, Datum=$datum, Kategoria=$kat,
            Popis=$popis, Suma=$suma, Mena=$mena, Doklad=$doklad, Poznamka=$pozn WHERE Id=$id";
        SetNakladParams(cmd, n);
        cmd.Parameters.AddWithValue("$id", n.Id);
        cmd.ExecuteNonQuery();
    }

    public void VymazNaklad(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Naklad WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private static void SetNakladParams(SqliteCommand cmd, Naklad n)
    {
        cmd.Parameters.AddWithValue("$aid", n.AutoId);
        cmd.Parameters.AddWithValue("$zid", (object?)n.ServisnyZaznamId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$datum", n.Datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$kat", n.Kategoria);
        cmd.Parameters.AddWithValue("$popis", (object?)n.Popis ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$suma", n.Suma);
        cmd.Parameters.AddWithValue("$mena", n.Mena);
        cmd.Parameters.AddWithValue("$doklad", (object?)n.Doklad ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$pozn", (object?)n.Poznamka ?? DBNull.Value);
    }

    private static Naklad MapNaklad(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        AutoId = r.GetInt32(1),
        ServisnyZaznamId = r.IsDBNull(2) ? null : r.GetInt32(2),
        Datum = DateTime.Parse(r.GetString(3)),
        Kategoria = r.GetString(4),
        Popis = r.IsDBNull(5) ? "" : r.GetString(5),
        Suma = (decimal)r.GetDouble(6),
        Mena = r.IsDBNull(7) ? "EUR" : r.GetString(7),
        Doklad = r.IsDBNull(8) ? "" : r.GetString(8),
        Poznamka = r.IsDBNull(9) ? "" : r.GetString(9),
        AutoNazov = r.GetString(10)
    };

    // ==================== SERVISNE TERMINY ====================

    public List<ServisnyTermin> GetServisneTerminy(int? autoId = null, bool lenAktivne = false)
    {
        var list = new List<ServisnyTermin>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        var where = new List<string>();
        if (autoId.HasValue) where.Add("t.AutoId=$autoId");
        if (lenAktivne) where.Add("t.JeAktivny=1");
        cmd.CommandText = @"
            SELECT t.*, a.Znacka || ' ' || a.Model || ' (' || a.SPZ || ')' as AutoNazov
            FROM ServisnyTermin t JOIN Auto a ON t.AutoId=a.Id
            " + (where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : "") +
            " ORDER BY t.DatumTerminu ASC";
        if (autoId.HasValue) cmd.Parameters.AddWithValue("$autoId", autoId.Value);
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) list.Add(MapTermin(reader));
        return list;
    }

    public int PridajTermin(ServisnyTermin t)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO ServisnyTermin (AutoId, Nazov, Popis, DatumTerminu, KMTerminu, OpakovanieDni, OpakovaniKM, JeAktivny, PosledneVykonanie, KMPoslednehoVykonania, Poznamka)
            VALUES ($aid, $nazov, $popis, $datum, $km, $opakDni, $opakKM, $aktivny, $posl, $poslKM, $pozn);
            SELECT last_insert_rowid();";
        SetTerminParams(cmd, t);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void AktualizujTermin(ServisnyTermin t)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE ServisnyTermin SET AutoId=$aid, Nazov=$nazov, Popis=$popis, DatumTerminu=$datum,
            KMTerminu=$km, OpakovanieDni=$opakDni, OpakovaniKM=$opakKM, JeAktivny=$aktivny,
            PosledneVykonanie=$posl, KMPoslednehoVykonania=$poslKM, Poznamka=$pozn WHERE Id=$id";
        SetTerminParams(cmd, t);
        cmd.Parameters.AddWithValue("$id", t.Id);
        cmd.ExecuteNonQuery();
    }

    public void VymazTermin(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM ServisnyTermin WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    public void OznacTerminVykonany(int id, DateTime datum, int km)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE ServisnyTermin SET PosledneVykonanie=$datum, KMPoslednehoVykonania=$km,
            DatumTerminu = CASE WHEN OpakovanieDni IS NOT NULL THEN date($datum, '+' || OpakovanieDni || ' days') ELSE DatumTerminu END,
            KMTerminu = CASE WHEN OpakovaniKM IS NOT NULL THEN $km + OpakovaniKM ELSE KMTerminu END
            WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.Parameters.AddWithValue("$datum", datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$km", km);
        cmd.ExecuteNonQuery();
    }

    private static void SetTerminParams(SqliteCommand cmd, ServisnyTermin t)
    {
        cmd.Parameters.AddWithValue("$aid", t.AutoId);
        cmd.Parameters.AddWithValue("$nazov", t.Nazov);
        cmd.Parameters.AddWithValue("$popis", (object?)t.Popis ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$datum", t.DatumTerminu.HasValue ? (object)t.DatumTerminu.Value.ToString("yyyy-MM-dd") : DBNull.Value);
        cmd.Parameters.AddWithValue("$km", (object?)t.KMTerminu ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$opakDni", (object?)t.OpakovanieDni ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$opakKM", (object?)t.OpakovaniKM ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$aktivny", t.JeAktivny ? 1 : 0);
        cmd.Parameters.AddWithValue("$posl", t.PosledneVykonanie.HasValue ? (object)t.PosledneVykonanie.Value.ToString("yyyy-MM-dd") : DBNull.Value);
        cmd.Parameters.AddWithValue("$poslKM", (object?)t.KMPoslednehoVykonania ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$pozn", (object?)t.Poznamka ?? DBNull.Value);
    }

    private static ServisnyTermin MapTermin(SqliteDataReader r) => new()
    {
        Id = r.GetInt32(0),
        AutoId = r.GetInt32(1),
        Nazov = r.GetString(2),
        Popis = r.IsDBNull(3) ? "" : r.GetString(3),
        DatumTerminu = r.IsDBNull(4) ? null : DateTime.Parse(r.GetString(4)),
        KMTerminu = r.IsDBNull(5) ? null : r.GetInt32(5),
        OpakovanieDni = r.IsDBNull(6) ? null : r.GetInt32(6),
        OpakovaniKM = r.IsDBNull(7) ? null : r.GetInt32(7),
        JeAktivny = r.GetInt32(8) == 1,
        PosledneVykonanie = r.IsDBNull(9) ? null : DateTime.Parse(r.GetString(9)),
        KMPoslednehoVykonania = r.IsDBNull(10) ? null : r.GetInt32(10),
        Poznamka = r.IsDBNull(11) ? "" : r.GetString(11),
        AutoNazov = r.GetString(12)
    };

    // ==================== KM ZAZNAMY ====================

    public List<KMZaznam> GetKMZaznamy(int autoId)
    {
        var list = new List<KMZaznam>();
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT k.*, a.Znacka || ' ' || a.Model || ' (' || a.SPZ || ')' as AutoNazov
            FROM KMZaznam k JOIN Auto a ON k.AutoId=a.Id
            WHERE k.AutoId=$autoId ORDER BY k.Datum DESC, k.StavKM DESC";
        cmd.Parameters.AddWithValue("$autoId", autoId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new KMZaznam
            {
                Id = reader.GetInt32(0),
                AutoId = reader.GetInt32(1),
                Datum = DateTime.Parse(reader.GetString(2)),
                StavKM = reader.GetInt32(3),
                Poznamka = reader.IsDBNull(4) ? "" : reader.GetString(4),
                AutoNazov = reader.GetString(5)
            });
        return list;
    }

    public void PridajKMZaznam(KMZaznam z)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO KMZaznam (AutoId, Datum, StavKM, Poznamka)
            VALUES ($aid, $datum, $km, $pozn)";
        cmd.Parameters.AddWithValue("$aid", z.AutoId);
        cmd.Parameters.AddWithValue("$datum", z.Datum.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("$km", z.StavKM);
        cmd.Parameters.AddWithValue("$pozn", (object?)z.Poznamka ?? DBNull.Value);
        cmd.ExecuteNonQuery();
        AktualizujKMAuta(z.AutoId, z.StavKM);
    }

    public void VymazKMZaznam(int id)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM KMZaznam WHERE Id=$id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    private void AktualizujKMAuta(int autoId, int km)
    {
        using var conn = GetConnection();
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Auto SET AktualneKM=MAX(AktualneKM, $km) WHERE Id=$id";
        cmd.Parameters.AddWithValue("$km", km);
        cmd.Parameters.AddWithValue("$id", autoId);
        cmd.ExecuteNonQuery();
    }
}
