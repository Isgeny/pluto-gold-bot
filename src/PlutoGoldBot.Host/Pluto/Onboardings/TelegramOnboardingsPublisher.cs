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

    public TelegramOnboardingsPublisher(ITreasuryService treasuryService, ITelegramBotClient telegramBotClient, IOptions<AppSettings> settings)
    {
        _treasuryService = treasuryService;
        _telegramBotClient = telegramBotClient;
        _settings = settings;
    }

    public async Task PublishOnboardings(ICollection<Onboarding> onboardings)
    {
        foreach (var onboarding in onboardings)
        {
            var messageText = await GetMessageText(onboarding);
            await _telegramBotClient.SendTextMessageAsync(_settings.Value.TELEGRAM_GROUP_ID, messageText, ParseMode.MarkdownV2, disableNotification: true, disableWebPagePreview: true);
        }
    }

    private async Task<string> GetMessageText(Onboarding onboarding)
    {
        var sb = new StringBuilder(1024);

        sb.AppendLine($"Onboarding: *{onboarding.Asset.ToEscapedString()} {onboarding.AssetName}*");
        sb.AppendLine($"Issued: 🟠 *{onboarding.Issued.ToEscapedString()} PLUTO*");
        sb.AppendLine($"Transaction: [wavesexplorer](https://wavesexplorer.com/transactions/{onboarding.TransactionId})");

        var plutoSupply = await _treasuryService.GetPlutoSupply();
        sb.AppendLine($"PLUTO Supply: 🟠 *{plutoSupply.ToEscapedString()}*");

        var treasuryValue = await _treasuryService.GetTreasuryValue();
        sb.AppendLine($"Treasury Value: 💵 *{treasuryValue.ToEscapedString()}*");

        var backedPrice = await _treasuryService.GetBackedPrice();
        sb.AppendLine($"Backed Price: 💵 *{backedPrice.ToEscapedString(2)}*");

        var marketPrice = await _treasuryService.GetMarketPrice();
        sb.AppendLine($"Market Price: 💵 *{marketPrice.ToEscapedString(2)}*");

        var maxPrice = await _treasuryService.GetMaxPrice();
        sb.AppendLine($"Max Price: 💵 *{maxPrice.ToEscapedString(2)}*");

        var growthFactor = await _treasuryService.GetGrowthFactor();
        sb.AppendLine($"Growth Factor: *{growthFactor.ToEscapedString()}%*");

        var emoji = (int)(onboarding.Issued / _settings.Value.EMOJI_COST);
        for (var i = 0; i < emoji; i++)
        {
            sb.Append("🟠");
        }

        return sb.ToString();
    }
}