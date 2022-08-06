namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class Args
{
    public string Type { get; set; } = string.Empty;

    public object Value { get; set; } = new();
}