var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();

builder.AddAzureFunctionsProject<Projects.Blog_Portfolio_Host>("host")
    .WithHostStorage(storage)
    .WithExternalHttpEndpoints();

await builder.Build().RunAsync();
