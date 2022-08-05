using PlutoGoldBot.Host.NodeApi.Blocks;

namespace PlutoGoldBot.Host.NodeApi;

public interface INodeApi
{
    [Get("/blocks/height")]
    Task<BlockHeight> GetBlockchainHeight();

    [Get("/blocks/at/{height}")]
    Task<Block> GetBlock(int height);
}