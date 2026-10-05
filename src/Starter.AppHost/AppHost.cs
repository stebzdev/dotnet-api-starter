using Microsoft.Extensions.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", secret: true);

var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume()
    .WithPgAdmin();

var StarterDb = postgres.AddDatabase("StarterDb", "Starter");

var keyclock = builder.AddKeycloak("keycloak", port: 8080)
    .WithImage("keycloak/keycloak")
    .WithImageRegistry("quay.io")
    .WithImageTag("26.8.0")
    .WithRealmImport("../../keycloak/import")
    .WithDataVolume();

var redis = builder.AddRedis("cache");

var seq = builder.AddSeq("seq")
    .WithDataVolume()
    .ExcludeFromManifest()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y");

var api = builder.AddProject<Starter_Api>("api")
    .WithReference(StarterDb)
    .WithReference(keyclock)
    .WithReference(redis)
    .WithReference(seq)
    .WaitFor(StarterDb)
    .WaitFor(keyclock)
    .WaitFor(redis)
    .WaitFor(seq);

if (builder.Environment.IsDevelopment())
{
    api.WithEnvironment("OTEL_DOTNET_EXPERIMENTAL_EFCORE_ENABLE_TRACE_DB_QUERY_PARAMETERS", "true");
    api.WithEnvironment("OTEL_SEMCONV_STABILITY_OPT_IN", "database");
}

var migrations = api.AddEFMigrations("Starter-migrations")
    .WithMigrationsProject<Starter_Application>()
    .WithReference(StarterDb)
    .WaitFor(StarterDb)
    .RunDatabaseUpdateOnStart();

api.WaitForCompletion(migrations);
 

await builder.Build().RunAsync();
