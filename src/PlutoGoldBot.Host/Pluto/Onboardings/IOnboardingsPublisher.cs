namespace PlutoGoldBot.Host.Pluto.Onboardings;

public interface IOnboardingsPublisher
{
    Task PublishOnboardings(ICollection<Onboarding> onboardings);
}