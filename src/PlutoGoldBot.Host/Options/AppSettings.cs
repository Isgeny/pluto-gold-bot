namespace PlutoGoldBot.Host.Options;

// ReSharper disable InconsistentNaming
public class AppSettings
{
    public string NODE_REST_URL { get; set; } = string.Empty;

    public long TELEGRAM_GROUP_ID { get; set; }

    public string TELEGRAM_TOKEN { get; set; } = string.Empty;
}