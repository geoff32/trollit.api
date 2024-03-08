using NpgsqlTypes;

namespace TrollIt.Infrastructure.Profiles.Models;

internal record Troll
(

        [PgName("id")]
        int Id,

        [PgName("name")]
        string Name,

        [PgName("breed")]
        string Breed,

        [PgName("shortbreed")]
        string Shortbreed,

        [PgName("level")]
        int Level,

        [PgName("profile")]
        Profile Profile
)
{
    public Troll(Scripts.Models.Troll troll, Scripts.Models.Caracs caracs)
        : this
        (
            Id: troll.Id,
            Name: troll.Nom,
            Breed: troll.Race,
            Shortbreed: troll.RaceNomCourt,
            Level: troll.Niveau,
            Profile: new Profile(caracs))
    {
        
    }
}