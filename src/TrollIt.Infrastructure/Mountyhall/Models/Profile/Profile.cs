namespace TrollIt.Infrastructure.Mountyhall.Models.Profile;

internal record Profile(
    Troll Troll,
    Situation Situation,
    Caracs Caracs,
    IEnumerable<Talent> Sorts,
    IEnumerable<Talent> Competences
);