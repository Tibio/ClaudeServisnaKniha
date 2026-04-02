# create_icon.ps1
# Vytvori app.ico pre ServisnaKniha — ikona otvorenej knihy
# Spustenie: powershell -ExecutionPolicy Bypass -File create_icon.ps1

Add-Type -AssemblyName System.Drawing

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$outPath = Join-Path $scriptDir "ServisnaKniha\app.ico"

# ---------------------------------------------------------------
# Nakreslenie ikony knihy na bitmapu zadanej velkosti
# ---------------------------------------------------------------
function Draw-BookIcon {
    param ([int]$size)

    $bmp = New-Object System.Drawing.Bitmap($size, $size, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g   = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode    = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.PixelOffsetMode   = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

    $s = $size / 32.0   # meritko (zaklad je 32x32)

    # Pozadie — temno-modra
    $bgColor = [System.Drawing.Color]::FromArgb(255, 30, 58, 95)
    $g.Clear($bgColor)

    # Zaoblene rohy pozadia
    $radius = [int](4 * $s)
    $bgPath = New-Object System.Drawing.Drawing2D.GraphicsPath
    $bgPath.AddArc(0, 0, $radius*2, $radius*2, 180, 90)
    $bgPath.AddArc($size - $radius*2, 0, $radius*2, $radius*2, 270, 90)
    $bgPath.AddArc($size - $radius*2, $size - $radius*2, $radius*2, $radius*2, 0, 90)
    $bgPath.AddArc(0, $size - $radius*2, $radius*2, $radius*2, 90, 90)
    $bgPath.CloseFigure()
    $bgBrush = New-Object System.Drawing.SolidBrush($bgColor)
    $g.Clear([System.Drawing.Color]::Transparent)
    $g.FillPath($bgBrush, $bgPath)

    # Lava strana knihy
    $lx = [int](4  * $s); $ly = [int](5 * $s)
    $lw = [int](10 * $s); $lh = [int](22 * $s)
    $pageBrush  = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(245, 245, 245))
    $g.FillRectangle($pageBrush, $lx, $ly, $lw, $lh)

    # Prava strana knihy
    $rx = [int](18 * $s); $ry = [int](5 * $s)
    $rw = [int](10 * $s); $rh = [int](22 * $s)
    $g.FillRectangle($pageBrush, $rx, $ry, $rw, $rh)

    # Chrbat knihy — zlata farba
    $spineColor  = [System.Drawing.Color]::FromArgb(255, 200, 60)
    $spineBrush  = New-Object System.Drawing.SolidBrush($spineColor)
    $spinePen    = New-Object System.Drawing.Pen($spineColor, [float]($s))
    $sx = [int](13.5 * $s); $sw = [int](5 * $s)
    $g.FillRectangle($spineBrush, $sx, $ly, $sw, $lh)

    # Tiene stranok (tenka ciara navrchu a na krajoch)
    $shadowPen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(80, 0, 0, 0), [float]($s * 0.7))
    $g.DrawRectangle($shadowPen, $lx, $ly, $lw - 1, $lh - 1)
    $g.DrawRectangle($shadowPen, $rx, $ry, $rw - 1, $rh - 1)

    # Riadky na lavej strane (iba pre vacsi rozmer)
    if ($size -ge 24) {
        $linePen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(120, 100, 120, 180), [float]($s * 0.6))
        $lineCount = if ($size -ge 48) { 6 } else { 4 }
        $step = ($lh - [int](4*$s)) / ($lineCount + 1)
        for ($i = 1; $i -le $lineCount; $i++) {
            $y = $ly + [int](2*$s) + [int]($i * $step)
            $g.DrawLine($linePen, $lx + [int](2*$s), $y, $lx + $lw - [int](2*$s), $y)
            $g.DrawLine($linePen, $rx + [int](2*$s), $y, $rx + $rw - [int](2*$s), $y)
        }
        $linePen.Dispose()
    }

    # Mala auto ikona — stredovy kruh (iba 48+ px)
    if ($size -ge 48) {
        $cx = [int](16 * $s); $cy = [int](24 * $s); $cr = [int](3.5 * $s)
        $circleBrush = New-Object System.Drawing.SolidBrush($spineColor)
        $g.FillEllipse($circleBrush, $cx - $cr, $cy - $cr, $cr*2, $cr*2)
        $circleBrush.Dispose()
    }

    # Upratanie
    $shadowPen.Dispose(); $spineBrush.Dispose(); $spinePen.Dispose()
    $pageBrush.Dispose(); $bgBrush.Dispose()
    $g.Dispose()
    return $bmp
}

# ---------------------------------------------------------------
# Ulozenie ICO suboru (PNG-based, Windows Vista+)
# ---------------------------------------------------------------
function Save-IcoFile {
    param ([System.Drawing.Bitmap[]]$bitmaps, [string]$filePath)

    $images = @()
    foreach ($bmp in $bitmaps) {
        $ms = New-Object System.IO.MemoryStream
        $bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
        $images += [PSCustomObject]@{
            Data   = $ms.ToArray()
            Width  = $bmp.Width
            Height = $bmp.Height
        }
        $ms.Dispose()
    }

    $fs     = [System.IO.File]::Create($filePath)
    $writer = New-Object System.IO.BinaryWriter($fs)

    # ICONDIR hlavicka
    $writer.Write([uint16]0)               # Reserved
    $writer.Write([uint16]1)               # Type = ICO
    $writer.Write([uint16]$images.Count)   # Count

    # Vypocet offsetu prveho obrazka: 6 + 16*count
    $offset = [uint32](6 + 16 * $images.Count)

    # ICONDIRENTRY pre kazdy obrazok
    foreach ($img in $images) {
        $w = if ($img.Width  -eq 256) { [byte]0 } else { [byte]$img.Width  }
        $h = if ($img.Height -eq 256) { [byte]0 } else { [byte]$img.Height }
        $writer.Write($w)                         # Width
        $writer.Write($h)                         # Height
        $writer.Write([byte]0)                    # ColorCount
        $writer.Write([byte]0)                    # Reserved
        $writer.Write([uint16]1)                  # Planes
        $writer.Write([uint16]32)                 # BitCount
        $writer.Write([uint32]$img.Data.Length)   # BytesInRes
        $writer.Write($offset)                    # ImageOffset
        $offset += [uint32]$img.Data.Length
    }

    # Obrazkove data
    foreach ($img in $images) {
        $writer.Write($img.Data)
    }

    $writer.Close()
    $fs.Close()
}

# ---------------------------------------------------------------
# Generovanie a ulozenie
# ---------------------------------------------------------------
Write-Host "Generujem ikonu knihy..."

$sizes   = @(16, 24, 32, 48, 64, 128, 256)
$bitmaps = $sizes | ForEach-Object { Draw-BookIcon -size $_ }

Save-IcoFile -bitmaps $bitmaps -filePath $outPath

foreach ($b in $bitmaps) { $b.Dispose() }

Write-Host "Ikona ulozena: $outPath"
Write-Host "Vygenerovane velkosti: $($sizes -join ', ') px"
