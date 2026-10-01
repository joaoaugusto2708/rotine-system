var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LifeQuest_Api>("lifequest-api");

builder.Build().Run();
