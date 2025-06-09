using MaterialQueueProcessAzure;
using MaterialQueueProcessAzure.DAL;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IRepository, Repository>();

var host = builder.Build();
host.Run();
