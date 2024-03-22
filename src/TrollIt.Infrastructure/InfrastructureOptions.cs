using TrollIt.Infrastructure.Mountyhall;

namespace TrollIt.Infrastructure;

public record InfrastructureOptions(string ConnectionString, MountyhallOptions Mountyhall);
