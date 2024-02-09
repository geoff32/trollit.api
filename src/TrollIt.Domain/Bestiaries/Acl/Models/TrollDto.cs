namespace TrollIt.Domain.Bestiaries.Acl.Models;

public record class TrollDto(int Id, string Name, BreedDto Breed, GuildDto? Guild);
