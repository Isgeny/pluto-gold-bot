using System.Text;
using PlutoGoldBot.Host.Extensions;
using PlutoGoldBot.Host.Pluto.Treasury;
using PlutoGoldBot.Host.Settings;
using Telegram.Bot.Types.Enums;

namespace PlutoGoldBot.Host.Pluto.Onboardings;

public class TelegramOnboardingsPublisher : IOnboardingsPublisher
{
    private readonly ITreasuryService _treasuryService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IOptions<AppSettings> _settings;
    private readonly ILogger<TelegramOnboardingsPublisher> _logger;

    public TelegramOnboardingsPublisher(ITreasuryService treasuryService, ITelegramBotClient telegramBotClient, IOptions<AppSettings> settings, ILogger<TelegramOnboardingsPublisher> logger)
    {
        _treasuryService = treasuryService;
        _telegramBotClient = telegramBotClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task PublishOnboardings(ICollection<Onboarding> onboardings)
    {
        var telegramGroupIds = _settings.Value.TELEGRAM_GROUP_IDS.Split(";").Select(long.Parse).ToHashSet();

        foreach (var onboarding in onboardings)
        {
            var messageText = await GetMessageText(onboarding);

            foreach (var telegramGroupId in telegramGroupIds)
            {
                await _telegramBotClient.SendTextMessageAsync(telegramGroupId, messageText, ParseMode.MarkdownV2, disableNotification: true, disableWebPagePreview: true);
            }

            _logger.LogInformation("Onboarding sent {Id}", onboarding.TransactionId);
        }
    }

    private async Task<string> GetMessageText(Onboarding onboarding)
    {
        var sb = new StringBuilder(1024);

        sb.AppendLine($"Onboarding: *{onboarding.Asset.ToEscapedQuantityString()} {onboarding.Asset.ToEscapedNameString()}*");
        sb.AppendLine($"Issued: 🟠 *{onboarding.Issued.ToEscapedQuantityString()} {onboarding.Issued.ToEscapedNameString()}*");
        sb.AppendLine($"Transaction: [wavesexplorer](https://wavesexplorer.com/transactions/{onboarding.TransactionId})");

        var plutoSupply = await _treasuryService.GetPlutoSupply();
        sb.AppendLine($"PLUTO Supply: 🟠 *{plutoSupply.ToEscapedQuantityString()}*");

        var treasuryValue = await _treasuryService.GetTreasuryValue();
        sb.AppendLine($"Treasury Value: 💵 *{treasuryValue.ToEscapedQuantityString()}*");

        var backedPrice = await _treasuryService.GetBackedPrice();
        sb.AppendLine($"Backed Price: 💵 *{backedPrice.ToEscapedQuantityString(2)}*");

        var marketPrice = await _treasuryService.GetMarketPrice();
        sb.AppendLine($"Market Price: 💵 *{marketPrice.ToEscapedQuantityString(2)}*");

        var maxPrice = await _treasuryService.GetMaxPrice();
        sb.AppendLine($"Max Price: 💵 *{maxPrice.ToEscapedQuantityString(2)}*");

        var growthFactor = await _treasuryService.GetGrowthFactor();
        sb.AppendLine($"Growth Factor: *{growthFactor.ToEscapedString()}%*");

        var emoji = (int)(onboarding.Issued.Quantity / _settings.Value.EMOJI_COST);
        for (var i = 0; i < emoji; i++)
        {
            sb.Append("🟠");
        }

        return sb.ToString();
    }
}