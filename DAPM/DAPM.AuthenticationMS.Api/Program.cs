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
builder.Services.AddQueueMessageConsumer<PostUserRoleConsumer, PostUserRoleMessage>();


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
    string[] roleNames = { "Guest", "User", "Admin", "SuperAdmin" };

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
    var a = await userManager.CreateAsync(new IdentityUser { UserName = "admin" }, "Password1@");
    var s = await userManager.CreateAsync(new IdentityUser { UserName = "superadmin" }, "Password1@");
    var u = await userManager.CreateAsync(new IdentityUser { UserName = "user" }, "Password1@");
    var g = await userManager.CreateAsync(new IdentityUser { UserName = "guest" }, "Password1@");

    var admin = await userManager.FindByNameAsync("admin");
    var superadmin = await userManager.FindByNameAsync("superadmin");
    var user = await userManager.FindByNameAsync("user");
    var guest = await userManager.FindByNameAsync("guest");
    
    await userManager.AddToRoleAsync(admin, "Admin");
    await userManager.AddToRoleAsync(superadmin, "SuperAdmin");
    await userManager.AddToRoleAsync(user, "User");
    await userManager.AddToRoleAsync(guest, "Guest");
}
