using PlutoGoldBot.Host.Settings;

namespace PlutoGoldBot.Host.Pluto.Onboardings;

/// <summary>
/// Invokes onboarding transactions scanning periodically by timer
/// </summary>
public class OnboardingsPeriodicScanner : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OnboardingsPeriodicScanner> _logger;
    private readonly PeriodicTimer _timer;

    public OnboardingsPeriodicScanner(IServiceProvider serviceProvider, IOptions<AppSettings> settings, ILogger<OnboardingsPeriodicScanner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(settings.Value.SCAN_PERIOD));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            try
            {
                var onboardingsProvider = scope.ServiceProvider.GetRequiredService<IOnboardingsProvider>();
                var onboardings = await onboardingsProvider.GetRecentOnboardings();

                var onboardingsPublisher = scope.ServiceProvider.GetRequiredService<IOnboardingsPublisher>();
                await onboardingsPublisher.PublishOnboardings(onboardings);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong");
            }
        }
    }
}