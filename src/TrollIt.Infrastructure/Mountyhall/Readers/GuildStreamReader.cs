using TrollIt.Infrastructure.Mountyhall.Models;
using TrollIt.Infrastructure.Mountyhall.Readers.Abstractions;

namespace TrollIt.Infrastructure.Mountyhall.Readers;

internal class GuildStreamReader : IContentReader<IEnumerable<Guild>>
{
    public IEnumerable<Guild> Read(string content)
    {
        return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(row => new Guild(row)).ToList();
    }
}