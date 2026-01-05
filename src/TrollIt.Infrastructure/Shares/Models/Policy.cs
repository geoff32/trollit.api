using NpgsqlTypes;

namespace TrollIt.Infrastructure.Shares.Models;

internal record Policy()
{
    [PgName("id")]
    public Guid Id { get; init; } = Guid.Empty;
    [PgName("name")]
    public string Name { get; init; } = default!;
    [PgName("trolls")]
    public TrollShare[] Trolls { get; init; } = default!;

    public Policy(Guid id, string name, TrollShare[] trolls) : this()
    {
        Id = id;
        Name = name;
        Trolls = trolls;
    }
}
