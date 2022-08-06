namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class IdsRequest
{
    public string[] Ids { get; set; } = Array.Empty<string>();

    public static implicit operator IdsRequest(string[] ids) => new() { Ids = ids };
}