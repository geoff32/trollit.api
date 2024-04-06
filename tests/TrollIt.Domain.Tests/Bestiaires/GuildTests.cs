using TrollIt.Domain.Bestiaries;
using TrollIt.Domain.Bestiaries.Acl.Models;
using FluentAssertions;

namespace TrollIt.Domain.Tests.Bestiaires;

public class GuildTests
{
    [Fact]
    public void Guild_CreatesCorrectlyFromGuildDto()
    {
        // Arrange
        var guildDto = new GuildDto(Id: 1, Name: "TestGuild");

        // Act
        var guild = new Guild(guildDto);

        // Assert
        guild.Id.Should().Be(guildDto.Id);
        guild.Name.Should().Be(guildDto.Name);
    }
}