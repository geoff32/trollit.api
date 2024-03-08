using System.Data;
using Dapper;
using Npgsql;
using TrollIt.Domain.Profiles.Abstractions;
using TrollIt.Domain.Profiles.Acl.Abstractions;
using TrollIt.Domain.Profiles.Acl.Models;
using TrollIt.Domain.Profiles.Infrastructure;
using TrollIt.Infrastructure.Npgsql;
using TrollIt.Infrastructure.Profiles.Scripts;
using TrollIt.Infrastructure.Profiles.Scripts.Models;

namespace TrollIt.Infrastructure.Profiles;

internal class ProfilesRepository(NpgsqlDataSource dataSource, IJsonScripts jsonScripts, IProfilesAcl profilesAcl) : IProfilesRepository
{
    public Task<IProfile> GetProfileAsync(int trollId)
    {
        throw new NotImplementedException();
    }

    public async Task<IProfile> RefreshProfileAsync(int trollId, string token)
    {
        var profile = await jsonScripts.ProfileAsync(trollId, token);

        await RefreshTrollAsync(dataSource, new Models.Troll(profile.Troll, profile.Caracs));

        var profileDto = new ProfileDto
        (
            TrollId: profile.Troll.Id,
            TurnDuration: GetDuration(profile.Caracs.Dla),
            Vitality: GetValueAttribute(profile.Caracs.PvMax),
            View: GetValueAttribute(profile.Caracs.Vue),
            Attack: GetDiceAttribute(profile.Caracs.Att),
            Dodge: GetDiceAttribute(profile.Caracs.Esq),
            Damage: GetDiceAttribute(profile.Caracs.Deg),
            Armor: GetDiceAttribute(profile.Caracs.Arm),
            Regeneration: GetDiceAttribute(profile.Caracs.Reg),
            MagicMastery: GetValueAttribute(profile.Caracs.Mm),
            MagicResistance: GetValueAttribute(profile.Caracs.Rm)
        );

        return profilesAcl.ToDomain(profileDto);
    }

    private static async Task RefreshTrollAsync(NpgsqlDataSource dataSource, Models.Troll troll)
    {
        var connection = dataSource.CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync
        (
            "app.refresh_troll",
            new
            {
                ptroll = new CustomTypeParameter<Models.Troll>(troll, "app.troll")
            },
            commandType: CommandType.StoredProcedure
        );
    }

    private static DiceAttributeDto GetDiceAttribute(Carac carac) =>
        new(Value: (int)carac.Car, BonusMalus: new BonusMalusDto(Physical: (int)carac.Bmp, Magical: (int)carac.Bmm));

    private static ValueAttributeDto GetValueAttribute(Carac carac) =>
        new(Value: (int)carac.Car, BonusMalus: new BonusMalusDto(Physical: (int)carac.Bmp, Magical: (int)carac.Bmm));

    private static ValueAttributeDto<TimeSpan> GetDuration(Carac carac) =>
        new
        (
            Value: TimeSpan.FromMinutes((int)carac.Car),
            BonusMalus: new BonusMalusDto<TimeSpan>
            (
                Physical: TimeSpan.FromMinutes((int)carac.Bmp),
                Magical: TimeSpan.FromMinutes((int)carac.Bmm)
            )
        );
}
