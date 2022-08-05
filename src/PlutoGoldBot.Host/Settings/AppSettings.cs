﻿namespace PlutoGoldBot.Host.Settings;

// ReSharper disable InconsistentNaming
/// <summary>
/// Settings that used for application. Can be set from docker environment variables
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Waves node api url
    /// </summary>
    public string NODE_REST_URL { get; set; } = string.Empty;

    /// <summary>
    /// Rollback in blocks for scanning transactions
    /// </summary>
    public int ROLLBACK { get; set; }

    /// <summary>
    /// Blockchain scan period for onboarding transactions (in seconds)
    /// </summary>
    public int SCAN_PERIOD { get; set; }

    /// <summary>
    /// Telegram group id where notification will be sent
    /// </summary>
    public long TELEGRAM_GROUP_ID { get; set; }

    /// <summary>
    /// Telegram bot token
    /// </summary>
    public string TELEGRAM_TOKEN { get; set; } = string.Empty;
}