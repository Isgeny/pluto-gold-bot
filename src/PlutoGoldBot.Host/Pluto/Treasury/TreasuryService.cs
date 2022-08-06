using Microsoft.Extensions.Caching.Memory;
using PlutoGoldBot.Host.Blockchain;
using PlutoGoldBot.Host.NodeApi;
using PlutoGoldBot.Host.Settings;

namespace PlutoGoldBot.Host.Pluto.Treasury;

public class TreasuryService : ITreasuryService
{
    private readonly INodeApi _nodeApi;
    private readonly IMemoryCache _cache;
    private readonly IOptions<AppSettings> _settings;

    public TreasuryService(INodeApi nodeApi, IMemoryCache cache, IOptions<AppSettings> settings)
    {
        _nodeApi = nodeApi;
        _cache = cache;
        _settings = settings;
    }

    public Task<Asset> GetPlutoSupply() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetPlutoSupply)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var plutoAsset = await _nodeApi.GetAssetDetails(Asset.PlutoId);
        return Asset.FromRawPluto(plutoAsset.Quantity);
    });

    public Task<Asset> GetTreasuryValue() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetTreasuryValue)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var treasuryValue = await _nodeApi.GetEntry(PlutoContracts.Parameters, "last_treasuryValue");
        return Asset.FromRawUsdn(treasuryValue.GetValue<long>());
    });

    public Task<Asset> GetBackedPrice() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetBackedPrice)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var plutoSupply = await GetPlutoSupply();
        var treasuryValue = await GetTreasuryValue();
        return Asset.FromUsdn(treasuryValue.Quantity / plutoSupply.Quantity);
    });

    public Task<Asset> GetMarketPrice() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetMarketPrice)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var evaluation = await _nodeApi.GetEvaluation(PlutoContracts.Parameters, "getMarketPrice(false)");
        var marketPrice = evaluation.Result.Value.GetProperty("_2").GetProperty("value").GetInt64();
        return Asset.FromRawUsdn(marketPrice);
    });

    public Task<Asset> GetMaxPrice() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetMaxPrice)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var backedPrice = await GetBackedPrice();
        return Asset.FromUsdn(backedPrice.Quantity * 3);
    });

    public Task<decimal> GetGrowthFactor() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetGrowthFactor)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var marketPrice = await GetMarketPrice();
        var backedPrice = await GetBackedPrice();

        return marketPrice.Quantity / backedPrice.Quantity * 100;
    });
}