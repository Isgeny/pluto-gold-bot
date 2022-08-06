namespace PlutoGoldBot.Host.Blockchain;

public readonly struct Asset
{
    public const string PlutoId = "Ajso6nTTjptu2UHLx6hfSXVtHFtRBJCkKYd5SAyj7zf5";

    public const string UsdnId = "DG2xFkPdDwKUoBkzGAhQtLpSGzfXLiCYPEzeKH2Ad24p";

    public Asset(long quantity, byte decimals, string name, string id)
    {
        var scale = new decimal(1, 0, 0, false, decimals);
        Quantity = quantity * scale;
        Decimals = decimals;
        Name = name;
        Id = id;
    }

    private Asset(decimal quantity, byte decimals, string name, string id)
    {
        Quantity = quantity;
        Decimals = decimals;
        Name = name;
        Id = id;
    }

    public decimal Quantity { get; }

    public int Decimals { get; }

    public string Name { get; }

    public string Id { get; }

    public static Asset FromRawPluto(long quantity) => new(quantity, 8, "PLUTO", PlutoId);

    public static Asset FromPluto(decimal quantity) => new(quantity, 8, "PLUTO", PlutoId);

    public static Asset FromRawUsdn(long quantity) => new(quantity, 6, "USD-N", UsdnId);

    public static Asset FromUsdn(decimal quantity) => new(quantity, 6, "USD-N", UsdnId);

    public string ToEscapedQuantityString(int decimals = 0)
    {
        return Quantity.ToString("N" + decimals, CultureInfo.InvariantCulture).Replace(".", "\\.").Replace("-", "\\-");
    }

    public string ToEscapedNameString()
    {
        return Name.Replace("-", "\\-");
    }
}