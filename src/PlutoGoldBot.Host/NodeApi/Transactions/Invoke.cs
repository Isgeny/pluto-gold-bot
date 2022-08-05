namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class Invoke
{
    public string DApp { get; set; } = string.Empty;

    public Call Call { get; set; } = new();

    public Payment[] Payment { get; set; } = Array.Empty<Payment>();

    public StateChanges StateChanges { get; set; } = new();
}