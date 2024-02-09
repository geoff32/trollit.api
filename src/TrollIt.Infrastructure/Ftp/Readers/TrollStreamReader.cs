using TrollIt.Infrastructure.Ftp.Models;
using TrollIt.Infrastructure.Ftp.Readers.Abstractions;

namespace TrollIt.Infrastructure.Ftp.Readers;

internal class TrollStreamReader : IContentReader<IEnumerable<Troll>>
{
    public IEnumerable<Troll> Read(string content)
    {
        return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(MapTroll)
            .Where(troll => troll != null)
            .Select(troll => troll!).ToList();
    }

    private Troll? MapTroll(string row)
    {
        try
        {
            return new Troll(row);
        }
        catch { }

        return null;
    }
}