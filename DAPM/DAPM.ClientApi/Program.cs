using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using RabbitMQ.Client;
using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using DAPM.ClientApi.Consumers;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults;
using Microsoft.OpenApi.Models;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.WebHost.UseKestrel(o => o.Limits.MaxRequestBodySize = null);

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MultipartBoundaryLengthLimit = int.MaxValue;
    x.MultipartHeadersCountLimit = int.MaxValue;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});


// RabbitMQ
builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DAPM Client API", Version = "v1" });
});


builder.Services.AddQueueMessageConsumer<GetOrganizationsProcessResultConsumer, GetOrganizationsProcessResult>();
builder.Services.AddQueueMessageConsumer<PostItemResultConsumer, PostItemProcessResult>();
builder.Services.AddQueueMessageConsumer<GetRepositoriesProcessResultConsumer, GetRepositoriesProcessResult>();
builder.Services.AddQueueMessageConsumer<GetResourcesProcessResultConsumer, GetResourcesProcessResult>();
builder.Services.AddQueueMessageConsumer<GetPipelinesProcessResultConsumer, GetPipelinesProcessResult>();
builder.Services.AddQueueMessageConsumer<GetResourceFilesProcessResultConsumer, GetResourceFilesProcessResult>();
builder.Services.AddQueueMessageConsumer<CollabHandshakeProcessResultConsumer, CollabHandshakeProcessResult>();
builder.Services.AddQueueMessageConsumer<PostPipelineCommandProcessResultConsumer, PostPipelineCommandProcessResult>();
builder.Services.AddQueueMessageConsumer<GetPipelineExecutionStatusProcessResultConsumer, GetPipelineExecutionStatusRequestResult>();
builder.Services.AddQueueMessageConsumer<PostLoginResultConsumer,PostLoginProcessResult>();
builder.Services.AddQueueMessageConsumer<PostRegistrationResultConsumer,PostRegistrationProcessResult>();



// Add services to the container.


builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<IPipelineService, PipelineService>();
builder.Services.AddSingleton<ITicketService, TicketService>();
builder.Services.AddScoped<ISystemService, SystemService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey??????????????????????????")),
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false, 
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapDefaultEndpoints();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers().RequireAuthorization();;

app.Run();
