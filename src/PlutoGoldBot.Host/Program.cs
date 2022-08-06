using PlutoGoldBot.Host.NodeApi;
using PlutoGoldBot.Host.Pluto.Onboardings;
using PlutoGoldBot.Host.Pluto.Treasury;
using PlutoGoldBot.Host.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices((context, services) =>
{
    services
        .AddRefitClient<INodeApi>()
        .ConfigureHttpClient((serviceProvider, httpClient) =>
            httpClient.BaseAddress = new Uri(serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value.NODE_REST_URL));

    services
        .AddSingleton<ITelegramBotClient>(serviceProvider =>
            new TelegramBotClient(serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value.TELEGRAM_TOKEN));

    services.AddHostedService<OnboardingsPeriodicScanner>();

    services.AddScoped<IOnboardingsProvider, OnboardingsProvider>();
    services.AddScoped<IOnboardingsPublisher, TelegramOnboardingsPublisher>();
    services.AddScoped<ITreasuryService, TreasuryService>();

    services.Configure<AppSettings>(context.Configuration);

    services.AddMemoryCache();
});

builder.Build().Run();