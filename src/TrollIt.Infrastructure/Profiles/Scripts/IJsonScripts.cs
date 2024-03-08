using Refit;
using TrollIt.Infrastructure.Profiles.Scripts.Models;

namespace TrollIt.Infrastructure.Profiles.Scripts;

internal interface IJsonScripts
{
    [Get("/SP_Profil4.php")]
    Task<Profile> ProfileAsync([AliasAs("Numero")] int id, [AliasAs("Motdepasse")] string token);
}