namespace TrollIt.Domain.Profiles.Abstractions;

public interface IBonusMalus : IBonusMalus<int>
{
}

public interface IBonusMalus<out T>
{
    T Physical { get; }
    T Magical { get; }
}
