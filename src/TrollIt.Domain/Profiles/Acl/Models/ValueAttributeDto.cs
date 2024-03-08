namespace TrollIt.Domain.Profiles.Acl.Models;

public record ValueAttributeDto(int Value, BonusMalusDto BonusMalus)
    : ValueAttributeDto<int, BonusMalusDto>(Value, BonusMalus);

public record ValueAttributeDto<T>(T Value, BonusMalusDto<T> BonusMalus)
    : ValueAttributeDto<T, BonusMalusDto<T>>(Value, BonusMalus);

public record ValueAttributeDto<T, TBonusMalus>(T Value, TBonusMalus BonusMalus)
    where TBonusMalus: BonusMalusDto<T>;
