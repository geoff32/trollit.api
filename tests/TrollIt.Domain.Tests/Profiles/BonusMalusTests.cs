using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class BonusMalusTests
{
    [Fact]
    public void BonusMalus_CreatesCorrectlyFromBonusMalusDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);

        // Act
        var bonusMalus = new BonusMalus(bonusMalusDto);

        // Assert
        bonusMalus.Physical.Should().Be(bonusMalusDto.Physical);
        bonusMalus.Magical.Should().Be(bonusMalusDto.Magical);
    }
}