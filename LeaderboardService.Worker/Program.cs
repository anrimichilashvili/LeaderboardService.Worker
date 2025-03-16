using LeaderboardService.Application.Interfaces;
using LeaderboardService.Application.RabbitMq.Interfaces;
using LeaderboardService.Application.RabbitMq.Services;
using LeaderboardService.Application.Services;
using LeaderboardService.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<BetEventWorker>();

builder.Services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(builder.Configuration));
builder.Services.AddSingleton<IMessageConsumer, RabbitMqConsumer>();
builder.Services.AddSingleton<ILeaderboardService, InMemoryLeaderboardService>();


builder.Services.AddSingleton<IMessageProducer, RabbitMqProducer>();     
builder.Services.AddHostedService<LeaderboardScheduler>();

var host = builder.Build();
host.Run();
