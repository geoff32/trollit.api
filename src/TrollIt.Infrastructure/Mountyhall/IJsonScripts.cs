using Refit;
using TrollIt.Infrastructure.Mountyhall.Models.Profile;

namespace TrollIt.Infrastructure.Mountyhall;

internal interface IJsonScripts
{
    [Get("/SP_Profil4.php")]
    Task<Profile> ProfileAsync([AliasAs("Numero")] int id, [AliasAs("Motdepasse")] string token, CancellationToken cancellationToken);
}