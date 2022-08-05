namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class Entry
{
    public string Key { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public object Value { get; set; } = new();

    public T GetValue<T>() => (T)Value;
}