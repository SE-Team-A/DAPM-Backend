using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

// subscribe here to rabbitmq queues

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
