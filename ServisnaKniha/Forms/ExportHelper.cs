using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public static class ExportHelper
{
    // ==================== CSV EXPORT ====================

    public static void ExportujCSV(DataGridView dgv, string defaultFileName)
    {
        using var dlg = new SaveFileDialog
        {
            Title = "Uložiť tabuľku ako CSV",
            Filter = "CSV súbor (*.csv)|*.csv|Všetky súbory (*.*)|*.*",
            FileName = $"{defaultFileName}_{DateTime.Today:yyyy-MM-dd}.csv",
            DefaultExt = "csv"
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        try
        {
            var sb = new StringBuilder();

            // Hlavička
            var headers = dgv.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .Select(c => QuoteCSV(c.HeaderText));
            sb.AppendLine(string.Join(";", headers));

            // Riadky
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                var cells = dgv.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Visible)
                    .Select(c => QuoteCSV(row.Cells[c.Index].Value?.ToString() ?? ""));
                sb.AppendLine(string.Join(";", cells));
            }

            File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show($"Tabuľka bola exportovaná:\n{dlg.FileName}",
                "Export úspešný", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba pri exporte: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string QuoteCSV(string value)
    {
        if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }

    // ==================== TLAČ ====================

    public static void TlacTabuľku(DataGridView dgv, string nadpis)
    {
        var printer = new GridPrinter(dgv, nadpis);

        using var preview = new PrintPreviewDialog
        {
            Document = printer.PrintDocument,
            Text = $"Náhľad tlače — {nadpis}",
            WindowState = FormWindowState.Maximized
        };
        preview.ShowDialog();
    }

    // ==================== VNÚTORNÁ TRIEDA TLAČ ====================

    private class GridPrinter
    {
        private readonly DataGridView _dgv;
        private readonly string _nadpis;
        private int _currentRow = 0;
        private readonly List<DataGridViewColumn> _cols;

        public PrintDocument PrintDocument { get; }

        public GridPrinter(DataGridView dgv, string nadpis)
        {
            _dgv = dgv;
            _nadpis = nadpis;
            _cols = dgv.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).ToList();

            PrintDocument = new PrintDocument();
            PrintDocument.DefaultPageSettings.Landscape = true;
            PrintDocument.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
            PrintDocument.PrintPage += PrintPage;
            PrintDocument.BeginPrint += (s, e) => _currentRow = 0;
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics!;
            var bounds = e.MarginBounds;
            float y = bounds.Top;

            // Nadpis
            using var fontNadpis = new Font("Segoe UI", 13f, FontStyle.Bold);
            using var fontHlavicka = new Font("Segoe UI", 8f, FontStyle.Bold);
            using var fontData = new Font("Segoe UI", 8f);
            using var brushText = new SolidBrush(Color.Black);
            using var brushHlavicka = new SolidBrush(Color.FromArgb(52, 73, 94));
            using var brushHlavickaTxt = new SolidBrush(Color.White);
            using var brushAlt = new SolidBrush(Color.FromArgb(245, 247, 250));
            using var pen = new Pen(Color.FromArgb(200, 200, 200));

            // Nadpis a dátum
            g.DrawString(_nadpis, fontNadpis, brushText, bounds.Left, y);
            var datum = $"Vytlačené: {DateTime.Now:dd.MM.yyyy HH:mm}";
            var datumSize = g.MeasureString(datum, fontData);
            g.DrawString(datum, fontData, brushText, bounds.Right - datumSize.Width, y + 5);
            y += fontNadpis.Height + 8;

            // Šírky stĺpcov (proporcionálne)
            float totalWidth = bounds.Width;
            float[] colWidths = _cols.Select(c => (float)c.FillWeight / _cols.Sum(x => x.FillWeight) * totalWidth).ToArray();

            float rowH = 20f;

            // Hlavička tabuľky
            float x = bounds.Left;
            g.FillRectangle(brushHlavicka, bounds.Left, y, totalWidth, rowH);
            for (int i = 0; i < _cols.Count; i++)
            {
                var rect = new RectangleF(x + 2, y + 2, colWidths[i] - 4, rowH - 4);
                g.DrawString(_cols[i].HeaderText, fontHlavicka, brushHlavickaTxt, rect,
                    new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
                x += colWidths[i];
            }
            y += rowH;

            // Dátové riadky
            int totalRows = _dgv.Rows.Count;
            while (_currentRow < totalRows)
            {
                var row = _dgv.Rows[_currentRow];
                if (row.IsNewRow) { _currentRow++; continue; }

                if (y + rowH > bounds.Bottom) break;

                x = bounds.Left;
                if (_currentRow % 2 == 1)
                    g.FillRectangle(brushAlt, bounds.Left, y, totalWidth, rowH);

                for (int i = 0; i < _cols.Count; i++)
                {
                    var rect = new RectangleF(x + 2, y + 1, colWidths[i] - 4, rowH - 2);
                    g.DrawString(row.Cells[_cols[i].Index].Value?.ToString() ?? "", fontData, brushText, rect,
                        new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
                    x += colWidths[i];
                }

                g.DrawLine(pen, bounds.Left, y + rowH, bounds.Right, y + rowH);
                y += rowH;
                _currentRow++;
            }

            // Orámovanie tabuľky
            g.DrawRectangle(Pens.Gray, bounds.Left, bounds.Top + fontNadpis.Height + 8, totalWidth, y - (bounds.Top + fontNadpis.Height + 8));

            // Číslo strany
            var pageStr = $"Strana {e.PageNumber}";
            var pageSize = g.MeasureString(pageStr, fontData);
            g.DrawString(pageStr, fontData, brushText, bounds.Right - pageSize.Width, bounds.Bottom + 5);

            e.HasMorePages = _currentRow < totalRows;
        }
    }
}
