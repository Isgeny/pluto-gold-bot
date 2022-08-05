namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class StateChanges
{
    public Entry[] Data { get; set; } = Array.Empty<Entry>();

    public Transfer[] Transfers { get; set; } = Array.Empty<Transfer>();

    public Invoke[] Invokes { get; set; } = Array.Empty<Invoke>();
}