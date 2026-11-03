namespace ServisnaKniha.Forms;

internal static class NumericUpDownExt
{
    /// <summary>
    /// Nastaví hodnotu orezanú na rozsah Minimum–Maximum, aby hodnoty mimo rozsahu
    /// (napr. z importovanej databázy) nespôsobili ArgumentOutOfRangeException.
    /// </summary>
    public static void NastavHodnotu(this NumericUpDown num, decimal hodnota) =>
        num.Value = Math.Clamp(hodnota, num.Minimum, num.Maximum);
}
