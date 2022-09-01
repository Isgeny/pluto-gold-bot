using Microsoft.Extensions.Caching.Memory;
using PlutoGoldBot.Host.Blockchain;
using PlutoGoldBot.Host.NodeApi;
using PlutoGoldBot.Host.NodeApi.Transactions;
using PlutoGoldBot.Host.Settings;

namespace PlutoGoldBot.Host.Pluto.Onboardings;

public class OnboardingsProvider : IOnboardingsProvider
{
    private readonly INodeApi _nodeApi;
    private readonly IMemoryCache _cache;
    private readonly IOptions<AppSettings> _settings;
    private const string IssuePlutoFunctionName = "issuePluto";
    private const string StartOnboardingFunctionName = "startOnboarding";

    public OnboardingsProvider(INodeApi nodeApi, IMemoryCache cache, IOptions<AppSettings> settings)
    {
        _nodeApi = nodeApi;
        _cache = cache;
        _settings = settings;
    }

    public async Task<ICollection<Onboarding>> GetRecentOnboardings()
    {
        var onboardings = new List<Onboarding>();
        var transactionIds = await GetOnboardingTransactionIds();
        if (!transactionIds.Any())
        {
            return onboardings;
        }

        var transactions = await _nodeApi.GetTransactions(transactionIds);

        foreach (var transaction in transactions)
        {
            if (_cache.TryGetValue($"Onboarding_{transaction.Id}", out _))
            {
                continue;
            }

            var transactionData = transaction.GetInvokeScriptData();
            var issuedPluto = GetIssuedPluto(transactionData);

            if (issuedPluto.Quantity < _settings.Value.MIN_ISSUED_SCAN)
            {
                continue;
            }

            var onboardingAsset = await GetOnboardingAsset(transactionData);

            onboardings.Add(new Onboarding
            {
                TransactionId = transaction.Id,
                Asset = onboardingAsset,
                Issued = issuedPluto,
            });

            _cache.Set($"Onboarding_{transaction.Id}", transaction.Id, TimeSpan.FromHours(1));
        }

        return onboardings;
    }

    private async Task<string[]> GetOnboardingTransactionIds()
    {
        var blockchainHeight = await _nodeApi.GetBlockchainHeight();
        var rollbackHeight = blockchainHeight.Height - _settings.Value.ROLLBACK + 1;
        var blocks = await _nodeApi.GetBlocks(rollbackHeight, blockchainHeight.Height);
        var transactionIds = new List<string>();

        foreach (var block in blocks)
        {
            foreach (var transaction in block.Transactions)
            {
                if (transaction.Type is not TransactionType.InvokeScript)
                {
                    continue;
                }

                var transactionData = transaction.GetInvokeScriptData();
                if (transactionData.Call.Function is not IssuePlutoFunctionName)
                {
                    continue;
                }

                transactionIds.Add(transaction.Id);
            }
        }

        return transactionIds.ToArray();
    }

    private static Asset GetIssuedPluto(InvokeScriptData invokeScriptData)
    {
        var args = invokeScriptData.StateChanges.Invokes
            .First(x => x.Call.Function == StartOnboardingFunctionName)
            .Call.Args;

        var issuedRaw = ((JsonElement)args[1].Value).GetInt64();
        var issued = Asset.FromRawPluto(issuedRaw);
        var premiumPercent = ((JsonElement)args[4].Value).GetInt64() / 10000M;
        return Asset.FromPluto(issued.Quantity * (1M + premiumPercent));
    }

    private async Task<Asset> GetOnboardingAsset(InvokeScriptData invokeScriptData)
    {
        var payment = invokeScriptData.Payment.First();
        if (payment.AssetId is null)
        {
            return Asset.FromRawWaves(payment.Amount);
        }

        var assetDetails = await _nodeApi.GetAssetDetails(payment.AssetId);
        return new Asset(payment.Amount, assetDetails.Decimals, assetDetails.Name, assetDetails.AssetId);
    }
}