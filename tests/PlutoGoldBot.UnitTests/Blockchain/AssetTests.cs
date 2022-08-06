using FluentAssertions;
using FluentAssertions.Execution;
using PlutoGoldBot.Host.Blockchain;

namespace PlutoGoldBot.UnitTests.Blockchain;

public class AssetTests
{
    [Fact]
    public void Ctor()
    {
        var asset = new Asset(1234, 2, "Test", "FooId");

        using (new AssertionScope())
        {
            asset.Id.Should().Be("FooId");
            asset.Name.Should().Be("Test");
            asset.Decimals.Should().Be(2);
            asset.Quantity.Should().Be(12.34M);
        }
    }
}