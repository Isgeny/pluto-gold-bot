namespace PlutoGoldBot.Host.Settings;

public static class JsonSettings
{
    public static readonly JsonSerializerOptions CamelCaseSettings = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        MaxDepth = 128,
    };
}