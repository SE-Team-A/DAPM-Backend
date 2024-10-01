using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using DAPM.AuthenticationMS.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.AuthenticationMS.Api.Services;

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

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();
