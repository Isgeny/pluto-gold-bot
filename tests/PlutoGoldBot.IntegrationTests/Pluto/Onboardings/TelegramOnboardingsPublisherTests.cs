using PlutoGoldBot.Host.Blockchain;
using PlutoGoldBot.Host.NodeApi;
using PlutoGoldBot.Host.Pluto.Onboardings;
using PlutoGoldBot.Host.Pluto.Treasury;
using PlutoGoldBot.Host.Settings;
using Telegram.Bot;

namespace PlutoGoldBot.IntegrationTests.Pluto.Onboardings;

public class TelegramOnboardingsPublisherTests
{
    [Theory(Skip = "PASTE YOUR CREDENTIALS")]
    [InlineData("", 0)]
    public async Task PublishOnboardings(string telegramToken, long telegramGroupId)
    {
        var nodeApi = RestService.For<INodeApi>(PublicNodes.WavesNodes);
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var appSettings = new AppSettings
        {
            CACHE_EXPIRATION = 1,
            TELEGRAM_GROUP_ID = telegramGroupId,
            EMOJI_COST = 100,
        };
        var settings = new OptionsWrapper<AppSettings>(appSettings);
        var treasuryService = new TreasuryService(nodeApi, memoryCache, settings);
        var telegramOnboardingsPublisher = new TelegramOnboardingsPublisher(treasuryService, new TelegramBotClient(telegramToken), settings);

        var onboarding = new Onboarding
        {
            Asset = Asset.FromUsdn(12345.67M),
            Issued = Asset.FromPluto(4567.89M),
        };

        await telegramOnboardingsPublisher.PublishOnboardings(new[] { onboarding });
    }
}