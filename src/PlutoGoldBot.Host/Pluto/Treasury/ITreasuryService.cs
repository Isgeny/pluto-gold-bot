using PlutoGoldBot.Host.Blockchain;

namespace PlutoGoldBot.Host.Pluto.Treasury;

public interface ITreasuryService
{
    Task<Asset> GetPlutoSupply();

    Task<Asset> GetTreasuryValue();

    Task<Asset> GetBackedPrice();

    Task<Asset> GetMarketPrice();

    Task<Asset> GetMaxPrice();

    Task<decimal> GetGrowthFactor();
}