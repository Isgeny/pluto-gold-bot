using PlutoGoldBot.Host.NodeApi.Assets;
using PlutoGoldBot.Host.NodeApi.Blocks;
using PlutoGoldBot.Host.NodeApi.Evaluations;
using PlutoGoldBot.Host.NodeApi.Transactions;

namespace PlutoGoldBot.Host.NodeApi;

public interface INodeApi
{
    [Get("/assets/details/{assetId}")]
    Task<AssetDetails> GetAssetDetails(string assetId);

    [Get("/blocks/height")]
    Task<BlockHeight> GetBlockchainHeight();

    [Get("/blocks/seq/{from}/{to}")]
    Task<Block[]> GetBlocks(int from, int to);

    [Get("/addresses/data/{address}/{key}")]
    Task<Entry> GetEntry(string address, string key);

    [Post("/utils/script/evaluate/{address}")]
    Task<Evaluation> GetEvaluation(string address, [Body(true)] EvaluationExpression expression);

    [Post("/transactions/info")]
    Task<Transaction[]> GetTransactions([Body(true)] IdsRequest ids);
}