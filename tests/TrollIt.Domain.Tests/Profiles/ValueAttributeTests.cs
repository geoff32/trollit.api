using FluentAssertions;
using TrollIt.Domain.Profiles;
using TrollIt.Domain.Profiles.Acl.Models;

namespace TrollIt.Domain.Tests.Profiles;

public class ValueAttributeTests
{
    [Fact]
    public void ValueAttribute_CreatesCorrectlyFromValueAttributeDto()
    {
        // Arrange
        var bonusMalusDto = new BonusMalusDto(Physical: 2, Magical: 1);
        var valueAttributeDto = new ValueAttributeDto(Value: 10, BonusMalus: bonusMalusDto);

        // Act
        var valueAttribute = new ValueAttribute(valueAttributeDto);

        // Assert
        valueAttribute.Value.Should().Be(valueAttributeDto.Value);
        valueAttribute.BonusMalus.Physical.Should().Be(bonusMalusDto.Physical);
        valueAttribute.BonusMalus.Magical.Should().Be(bonusMalusDto.Magical);
    }
}