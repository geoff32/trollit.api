namespace TrollIt.Domain.Bestiaries.Acl.Models;

public record TrollDto(int Id, string Name, BreedDto Breed, GuildDto? Guild);
