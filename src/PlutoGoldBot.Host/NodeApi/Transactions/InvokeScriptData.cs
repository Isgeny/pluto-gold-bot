namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class InvokeScriptData
{
    public string DApp { get; set; } = string.Empty;

    public Payment[] Payment { get; set; } = Array.Empty<Payment>();

    public Call Call { get; set; } = new();

    public StateChanges StateChanges { get; set; } = new();
}