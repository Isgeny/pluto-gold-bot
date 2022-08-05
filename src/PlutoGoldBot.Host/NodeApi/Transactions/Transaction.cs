using PlutoGoldBot.Host.Settings;

namespace PlutoGoldBot.Host.NodeApi.Transactions;

public class Transaction
{
    public string Id { get; set; } = string.Empty;

    public int Type { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

    public InvokeScriptData GetInvokeScriptData() => new()
    {
        DApp = (string)Properties["dApp"],
        Payment = ((JsonElement)Properties["payment"]).Deserialize<Payment[]>(JsonSettings.CamelCaseSettings)!,
        Call = ((JsonElement)Properties["call"]).Deserialize<Call>(JsonSettings.CamelCaseSettings)!,
        StateChanges = Properties.ContainsKey("stateChanges")
            ? ((JsonElement)Properties["stateChanges"]).Deserialize<StateChanges>(JsonSettings.CamelCaseSettings)!
            : new StateChanges(),
    };
}