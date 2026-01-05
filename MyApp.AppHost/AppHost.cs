var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.MyApp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

// Add the Nuxt frontend as an npm project
var frontend = builder.AddNpmApp("frontend", "../MyApp.Web", "dev")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithEnvironment("NUXT_PUBLIC_API_BASE", apiService.GetEndpoint("http"));

builder.Build().Run();
