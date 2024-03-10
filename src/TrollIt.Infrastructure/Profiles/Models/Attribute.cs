using NpgsqlTypes;
using TrollIt.Infrastructure.Mountyhall.Models.Profile;

namespace TrollIt.Infrastructure.Profiles.Models;

internal record Attribute
(
    [PgName("value")]
    int Value,
    [PgName("physicalbonus")]
    int Physicalbonus,
    [PgName("magicalbonus")]
    int Magicalbonus
)
{
    public Attribute(Carac carac)
        : this
        (
            Value: (int)carac.Car,
            Physicalbonus: (int)carac.Bmp,
            Magicalbonus: (int)carac.Bmm
        )
    {
    }
}
