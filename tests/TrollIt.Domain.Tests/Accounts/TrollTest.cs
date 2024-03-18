using FluentAssertions;
using TrollIt.Domain.Accounts;
using TrollIt.Domain.Accounts.Acl.Models;

namespace TrollIt.Domain.Tests;

public class TrollTest
{
    [Fact]
    public void Create_FromDto_ShouldReturnTroll()
    {
        var trollDto = new TrollDto(1, "Name", "Token");
        var troll = new Troll(trollDto);

        troll.Should().BeEquivalentTo(trollDto);
    }
}
