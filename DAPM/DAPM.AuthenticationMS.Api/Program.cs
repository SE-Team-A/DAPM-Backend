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
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>

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
builder.Services.AddScoped<IRolesService, RolesService>();

builder.Services.AddQueueMessageConsumer<PostLoginConsumer, PostLoginMessage>();
builder.Services.AddQueueMessageConsumer<PostRegistrationConsumer, PostRegistrationMessage>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

// Call method to seed roles on startup
// Only for release 1

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    await SeedRolesAsync(roleManager);
    await CreateTmpAdmin(userManager);
}

app.Run();

async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "User", "Admin" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// temporary seed admin user
async Task CreateTmpAdmin(UserManager<IdentityUser> userManager)
{
    var result = await userManager.CreateAsync(new IdentityUser { UserName = "admin" }, "Password1@");

    if (!result.Succeeded) return;

    var user = await userManager.FindByNameAsync("admin");

    if (user == null) return;
    
    await userManager.AddToRoleAsync(user, "Admin");
}
