using Microsoft.Extensions.Caching.Memory;
using TrollIt.Domain;
using TrollIt.Domain.Bestiaries.Abstractions;
using TrollIt.Domain.Bestiaries.Acl.Abstractions;
using TrollIt.Domain.Bestiaries.Acl.Models;
using TrollIt.Domain.Bestiaries.Infrastructure;
using TrollIt.Infrastructure.Mountyhall;

namespace TrollIt.Infrastructure;

internal class TrollBestiary(IFtpClient ftpClient, IBestiariesAcl bestiariesAcl, IMemoryCache memoryCache) : ITrollBestiary
{
    public async Task<ITroll?> GetTrollAsync(int id)
    {
        var guilds = await memoryCache.GetOrCreateAsync("AllGuilds", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
            var guilds = await ftpClient.GetGuildsAsync();

            return guilds.ToDictionary(guild => guild.Id, guild => new GuildDto(guild.Id, guild.Name));
        });

        var trolls = await memoryCache.GetOrCreateAsync("AllTrolls", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
            var trolls = await ftpClient.GetTrollsAsync();

            return trolls.ToDictionary(
                troll => troll.Id,
                troll => bestiariesAcl
                    .ToDomain(new TrollDto(troll.Id, troll.Name, GetBreed(troll.Breed), GetGuild(troll.GuildId, guilds))
                )
            );
        });

        return trolls != null && trolls.TryGetValue(id, out var troll) ? troll : null;
    }

    private static GuildDto? GetGuild(int? guildId, Dictionary<int, GuildDto>? guilds)
        => guildId.HasValue && guilds != null && guilds.TryGetValue(guildId.Value, out var guild) ? guild : null;

    private static BreedDto GetBreed(string breed) => breed switch
    {
        "Skrim" => new BreedDto("Skrim", "S"),
        "Tomawak" => new BreedDto("Tomawak", "T"),
        "Darkling" => new BreedDto("Darkling", "G"),
        "Durakuir" => new BreedDto("Durakuir", "D"),
        "Kastar" => new BreedDto("Kastar", "K"),
        "Nkrwapu" => new BreedDto("Nkrwapu", "N"),
        _ => new BreedDto("Inconnu", "?"),
    };
}
