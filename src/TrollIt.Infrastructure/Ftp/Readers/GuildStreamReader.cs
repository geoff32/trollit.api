using TrollIt.Infrastructure.Ftp.Models;
using TrollIt.Infrastructure.Ftp.Readers.Abstractions;

namespace TrollIt.Infrastructure.Ftp.Readers;

internal class GuildStreamReader : IContentReader<IEnumerable<Guild>>
{
    public IEnumerable<Guild> Read(string content)
    {
        return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(row => new Guild(row)).ToList();
    }
}