namespace TrollIt.Domain.Profiles.Abstractions;

public interface ITurnDuration : IValueAttribute<TimeSpan, IBonusMalus<TimeSpan>>
{
}
