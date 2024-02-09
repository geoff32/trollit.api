using Refit;
using TrollIt.Infrastructure.Ftp.Models;

namespace TrollIt.Infrastructure.Ftp;

internal interface IFtpClient
{
    [Get("/Public_Trolls2.txt")]
    Task<IEnumerable<Troll>> GetTrollsAsync();

    [Get("/Public_Guildes.txt")]
    Task<IEnumerable<Guild>> GetGuildsAsync();
}