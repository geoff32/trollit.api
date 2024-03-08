using TrollIt.Domain.Bestiaries.Abstractions;

namespace TrollIt.Domain.Bestiaries.Infrastructure;

public interface ITrollBestiary
{
    Task<ITroll?> GetTroll(int id);
}
