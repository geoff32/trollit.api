namespace TrollIt.Infrastructure.Mountyhall.Models;

public record Guild(int Id, string Name)
{
    public Guild(string row)
        : this(row.Split(";"))
    { }
    public Guild(string[] columns)
        : this(int.Parse(columns[0]), columns[1])
    { }
}
