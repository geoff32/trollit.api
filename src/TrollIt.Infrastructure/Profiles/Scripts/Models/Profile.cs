namespace TrollIt.Infrastructure.Profiles.Scripts.Models;

internal record Profile(
    Troll Troll,
    Situation Situation,
    Caracs Caracs,
    IEnumerable<Talent> Sorts,
    IEnumerable<Talent> Competences
);