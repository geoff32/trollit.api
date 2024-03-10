namespace TrollIt.Infrastructure.Mountyhall.Models.Profile;

internal record Talent(int Id, string Nom, IEnumerable<int> Niveaux, int Bonus);
