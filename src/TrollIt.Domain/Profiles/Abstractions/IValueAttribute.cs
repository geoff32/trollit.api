namespace TrollIt.Domain.Profiles.Abstractions;

public interface IValueAttribute : IValueAttribute<int, IBonusMalus>
{
}

public interface IValueAttribute<out T, TBonusMalus>
    where TBonusMalus: IBonusMalus<T>
{
    T Value { get; }
    TBonusMalus BonusMalus { get; }
}
