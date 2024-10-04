using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using DAPM.AuthenticationMS.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DAPM.AuthenticationMS.Api.Consumers;
using DAPM.AuthenticationMS.Api.Services;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// subscribe here to rabbitmq queues


builder.Services.AddDbContext<AuthenticationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); }
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options => {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AuthenticationDbContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddControllers();

builder.Services.AddQueueMessageConsumer<PostLoginConsumer, PostLoginMessage>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();
