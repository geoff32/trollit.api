namespace TrollIt.Domain.Profiles.Abstractions;

public interface IVitality : IValueAttribute
{
    int Max { get; }
}
