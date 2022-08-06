using PlutoGoldBot.Host.NodeApi;
using PlutoGoldBot.Host.Pluto.Onboardings;
using PlutoGoldBot.Host.Settings;

namespace PlutoGoldBot.IntegrationTests.Pluto.Onboardings;

public class OnboardingsProviderTests
{
    [Fact]
    public async Task GetRecentOnboardings()
    {
        var nodeApi = RestService.For<INodeApi>(PublicNodes.WavesNodes);
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var settings = new OptionsWrapper<AppSettings>(new AppSettings { ROLLBACK = 100, MIN_ISSUED_SCAN = 1 });
        var onboardingsProvider = new OnboardingsProvider(nodeApi, memoryCache, settings);

        var onboardings = await onboardingsProvider.GetRecentOnboardings();

        onboardings.Should().NotBeNull();
    }
}