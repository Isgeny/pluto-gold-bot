namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class Transfer
{
    public string Address { get; set; } = string.Empty;

    public string? Asset { get; set; }

    public long Amount { get; set; }
}