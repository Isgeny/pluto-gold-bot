using PlutoGoldBot.Host.NodeApi;
using PlutoGoldBot.Host.Pluto.Treasury;
using PlutoGoldBot.Host.Settings;

namespace PlutoGoldBot.IntegrationTests.Pluto.Treasury;

public class TreasuryServiceTests
{
    private readonly ITreasuryService _treasuryService;

    public TreasuryServiceTests()
    {
        var nodeApi = RestService.For<INodeApi>(PublicNodes.WavesNodes);
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var settings = new OptionsWrapper<AppSettings>(new AppSettings { CACHE_EXPIRATION = 1 });
        _treasuryService = new TreasuryService(nodeApi, memoryCache, settings);
    }

    [Fact]
    public async Task GetPlutoSupply_NotZero()
    {
        var plutoSupply = await _treasuryService.GetPlutoSupply();

        plutoSupply.Should().NotBe(0);
    }

    [Fact]
    public async Task GetTreasuryValue_NotZero()
    {
        var treasuryValue = await _treasuryService.GetTreasuryValue();

        treasuryValue.Should().NotBe(0);
    }

    [Fact]
    public async Task GetBackedPrice_NotZero()
    {
        var backedPrice = await _treasuryService.GetBackedPrice();

        backedPrice.Should().NotBe(0);
    }

    [Fact]
    public async Task GetMarketPrice_NotZero()
    {
        var marketPrice = await _treasuryService.GetMarketPrice();

        marketPrice.Should().NotBe(0);
    }

    [Fact]
    public async Task GetMaxPrice_NotZero()
    {
        var maxPrice = await _treasuryService.GetMaxPrice();

        maxPrice.Should().NotBe(0);
    }

    [Fact]
    public async Task GetGrowthFactor_NotZero()
    {
        var growthFactor = await _treasuryService.GetGrowthFactor();

        growthFactor.Should().NotBe(0);
    }
}