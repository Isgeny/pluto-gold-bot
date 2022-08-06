namespace PlutoGoldBot.Host.Pluto.Onboardings;

public interface IOnboardingsProvider
{
    Task<ICollection<Onboarding>> GetRecentOnboardings();
}