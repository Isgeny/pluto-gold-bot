namespace PlutoGoldBot.Host.Extensions;

public static class Extensions
{
    public static string ToEscapedString(this decimal value, int decimals = 0)
    {
        return value.ToString("N" + decimals, CultureInfo.InvariantCulture).Replace(".", "\\.").Replace("-", "\\-");
    }
}