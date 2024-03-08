using NpgsqlTypes;
using TrollIt.Infrastructure.Profiles.Scripts.Models;

namespace TrollIt.Infrastructure.Profiles.Models;

internal record Profile
(
        [PgName("vitality")]
        Attribute Vitality,

        [PgName("view")]
        Attribute View,

        [PgName("attack")]
        Attribute Attack,

        [PgName("dodge")]
        Attribute Dodge,

        [PgName("damage")]
        Attribute Damage,

        [PgName("regeneration")]
        Attribute Regeneration,

        [PgName("armor")]
        Attribute Armor,

        [PgName("magicmastery")]
        Attribute Magicmastery,

        [PgName("magicresistance")]
        Attribute Magicresistance,

        [PgName("turnduration")]
        Attribute Turnduration
)
{
    public Profile(Caracs caracs)
        : this
        (
            Vitality: new Attribute(caracs.PvMax),
            View: new Attribute(caracs.Vue),
            Attack: new Attribute(caracs.Att),
            Dodge: new Attribute(caracs.Esq),
            Damage: new Attribute(caracs.Deg),
            Regeneration: new Attribute(caracs.Reg),
            Armor: new Attribute(caracs.Arm),
            Magicmastery: new Attribute(caracs.Mm),
            Magicresistance: new Attribute(caracs.Rm),
            Turnduration: new Attribute(caracs.Dla)
        )
    {
    }
}
