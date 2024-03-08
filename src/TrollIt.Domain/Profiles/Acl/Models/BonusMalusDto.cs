namespace TrollIt.Domain.Profiles.Acl.Models;

public record BonusMalusDto(int Physical, int Magical) : BonusMalusDto<int>(Physical, Magical);
public record BonusMalusDto<T>(T Physical, T Magical);