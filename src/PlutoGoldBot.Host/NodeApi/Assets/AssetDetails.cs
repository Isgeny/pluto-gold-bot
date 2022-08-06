namespace PlutoGoldBot.Host.NodeApi.Assets;

public class AssetDetails
{
    public string AssetId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public byte Decimals { get; set; }

    public long Quantity { get; set; }
}