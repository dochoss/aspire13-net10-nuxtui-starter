var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.MyApp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

// Add the Nuxt frontend as a JavaScript app (pnpm)
var frontend = builder.AddJavaScriptApp("frontend", "../MyApp.Web")
    .WithPnpm()
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithEnvironment("NUXT_PUBLIC_API_BASE", apiService.GetEndpoint("http"));

builder.Build().Run();
