namespace PlutoGoldBot.Host.Pluto.Onboardings;

public class Onboarding
{
    public string TransactionId { get; set; } = string.Empty;

    public decimal Asset { get; set; }

    public string AssetName { get; set; } = string.Empty;

    public decimal Issued { get; set; }
}