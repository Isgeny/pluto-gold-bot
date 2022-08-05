namespace PlutoGoldBot.Host.Pluto.Treasury;

public interface ITreasuryService
{
    Task<decimal> GetPlutoSupply();

    Task<decimal> GetTreasuryValue();

    Task<decimal> GetBackedPrice();

    Task<decimal> GetMarketPrice();

    Task<decimal> GetMaxPrice();

    Task<decimal> GetGrowthFactor();
}