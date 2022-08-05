using PlutoGoldBot.Host.NodeApi.Transactions;

namespace PlutoGoldBot.Host.NodeApi.Blocks;

public class Block
{
    public int Height { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
}