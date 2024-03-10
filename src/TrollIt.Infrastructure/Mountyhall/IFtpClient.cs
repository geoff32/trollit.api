using Refit;
using TrollIt.Infrastructure.Mountyhall.Models;

namespace TrollIt.Infrastructure.Mountyhall;

internal interface IFtpClient
{
    [Get("/Public_Trolls2.txt")]
    Task<IEnumerable<Troll>> GetTrollsAsync();

    [Get("/Public_Guildes.txt")]
    Task<IEnumerable<Guild>> GetGuildsAsync();
}