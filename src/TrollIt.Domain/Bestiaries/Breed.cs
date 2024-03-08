using TrollIt.Domain.Bestiaries.Abstractions;

namespace TrollIt.Domain.Bestiaries;

internal record Breed(string Name, string ShortName) : IBreed
{
    public Breed(BreedDto breedDto)
        : this(breedDto.Name, breedDto.ShortName)
    {
    }
}
