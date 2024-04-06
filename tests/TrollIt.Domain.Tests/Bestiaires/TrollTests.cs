using FluentAssertions;
using TrollIt.Domain.Bestiaries;
using TrollIt.Domain.Bestiaries.Acl.Models;
using TrollIt.Domain.Bestiaries.Abstractions;

namespace TrollIt.Domain.Tests.Bestiaries;
public class TrollTests
{
    [Fact]
    public void Troll_CreatesCorrectlyFromTrollDto()
    {
        // Arrange
        var breedDto = new BreedDto(Name: "TestBreed", ShortName: "TestShortName");
        var guildDto = new GuildDto(Id: 1, Name: "TestGuild");
        var trollDto = new TrollDto(Id: 1, Name: "TestTroll", Breed: breedDto, Guild: guildDto);

        // Act
        var troll = new Troll(trollDto);

        // Assert
        troll.Id.Should().Be(trollDto.Id);
        troll.Name.Should().Be(trollDto.Name);
        troll.Breed.Name.Should().Be(breedDto.Name);
        troll.Breed.ShortName.Should().Be(breedDto.ShortName);
        troll.Guild?.Id.Should().Be(guildDto.Id);
    }
}