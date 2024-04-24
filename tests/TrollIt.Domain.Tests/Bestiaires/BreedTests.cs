using FluentAssertions;
using TrollIt.Domain.Bestiaries;
using TrollIt.Domain.Bestiaries.Acl.Models;

namespace TrollIt.Domain.Tests.Bestiaires;

public class BreedTests
{
    [Fact]
    public void Breed_CreatesCorrectlyFromBreedDto()
    {
        // Arrange
        var breedDto = new BreedDto(Name: "TestName", ShortName: "TestShortName");

        // Act
        var breed = new Breed(breedDto);

        // Assert
        breed.Name.Should().Be(breedDto.Name);
        breed.ShortName.Should().Be(breedDto.ShortName);
    }
}