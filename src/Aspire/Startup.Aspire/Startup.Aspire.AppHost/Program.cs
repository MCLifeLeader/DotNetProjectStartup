var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var seq = builder.AddSeq("seq")
    .WithDataVolume();

var apiService = builder.AddProject<Projects.Startup_Aspire_ApiService>("apiservice")
    .WithReference(seq);

builder.AddProject<Projects.Startup_Aspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(seq);

builder.Build().Run();
