using PlutoGoldBot.Host.Blockchain;

namespace PlutoGoldBot.Host.Pluto.Onboardings;

public class Onboarding
{
    public string TransactionId { get; set; } = string.Empty;

    public Asset Asset { get; set; }

    public Asset Issued { get; set; }
}