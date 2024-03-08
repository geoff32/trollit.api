namespace TrollIt.Infrastructure.Profiles.Scripts.Models;

internal record Talent(int Id, string Nom, IEnumerable<int> Niveaux, int Bonus);
