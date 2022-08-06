namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class Call
{
    public string Function { get; set; } = string.Empty;

    public Args[] Args { get; set; } = Array.Empty<Args>();
}