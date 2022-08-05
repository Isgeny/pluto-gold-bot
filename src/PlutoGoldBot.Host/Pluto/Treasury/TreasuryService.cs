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

    public Task<decimal> GetPlutoSupply() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetPlutoSupply)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var plutoAsset = await _nodeApi.GetAssetDetails(Assets.PlutoId);
        return plutoAsset.Quantity / Assets.PlutoScale;
    });

    public Task<decimal> GetTreasuryValue() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetTreasuryValue)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var treasuryValue = await _nodeApi.GetEntry(PlutoContracts.Parameters, "last_treasuryValue");
        return treasuryValue.GetValue<long>() / Assets.UsdnScale;
    });

    public Task<decimal> GetBackedPrice() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetBackedPrice)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var plutoSupply = await GetPlutoSupply();
        var treasuryValue = await GetTreasuryValue();
        return treasuryValue / plutoSupply;
    });

    public Task<decimal> GetMarketPrice() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetMarketPrice)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var evaluation = await _nodeApi.GetEvaluation(PlutoContracts.Parameters, "getMarketPrice(false)");
        var marketPrice = evaluation.Result.Value.GetProperty("_2").GetProperty("value").GetInt64();
        return marketPrice / Assets.UsdnScale;
    });

    public Task<decimal> GetMaxPrice() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetMaxPrice)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var backedPrice = await GetBackedPrice();
        return backedPrice * 3;
    });

    public Task<decimal> GetGrowthFactor() => _cache.GetOrCreateAsync($"{nameof(TreasuryService)}_{nameof(GetGrowthFactor)}", async entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_settings.Value.CACHE_EXPIRATION));

        var marketPrice = await GetMarketPrice();
        var backedPrice = await GetBackedPrice();

        return marketPrice / backedPrice * 100;
    });
}