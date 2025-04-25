var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050))
    .WithDataVolume(isReadOnly: false);

var postgresdb = postgres.AddDatabase("postgresdb");


builder.AddProject<Projects.WebApi>("apiservice")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.Build().Run();