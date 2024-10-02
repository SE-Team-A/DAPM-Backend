using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using DAPM.AuthenticationMS.Api.Consumers;
using RabbitMQLibrary.Messages.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddQueueMessageConsumer<PostLoginConsumer, PostLoginMessage>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
