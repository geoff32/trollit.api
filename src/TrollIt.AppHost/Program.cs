var builder = DistributedApplication.CreateBuilder(args);

var db = builder
    .AddPostgres("trollit-db")
    .WithInitBindMount("../../postgres/init")
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin()
    .AddDatabase("postgres");

builder.AddProject<Projects.TrollIt_Api>("trollit-api")
    .WithReference(db)
    .WaitFor(db);

await builder.Build().RunAsync();