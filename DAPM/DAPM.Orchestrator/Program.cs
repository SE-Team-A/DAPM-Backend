using RabbitMQLibrary.Implementation;
using RabbitMQLibrary.Extensions;
using RabbitMQLibrary.Messages.ResourceRegistry;
using DAPM.Orchestrator.Consumers.StartProcessConsumers;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using DAPM.Orchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry;
using DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo;
using DAPM.Orchestrator.Services;
using DAPM.Orchestrator.Consumers.ResultConsumers.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using DAPM.Orchestrator.Consumers;
using RabbitMQLibrary.Messages.Orchestrator.Other;
using DAPM.Orchestrator.Consumers.ResultConsumers.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using DAPM.Orchestrator.Consumers.ResultConsumers.FromOperator;
using RabbitMQLibrary.Messages.ClientApi;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
/// <author>Tamas Drabos</author>

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQueueing(new QueueingConfigurationSettings
{
    RabbitMqConsumerConcurrency = 5,
    RabbitMqHostname = "rabbitmq",
    RabbitMqPort = 5672,
    RabbitMqPassword = "guest",
    RabbitMqUsername = "guest"
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

//START PROCESS REQUESTS
builder.Services.AddQueueMessageConsumer<GetOrganizationsRequestConsumer, GetOrganizationsRequest>();
builder.Services.AddQueueMessageConsumer<GetRepositoriesRequestConsumer, GetRepositoriesRequest>();
builder.Services.AddQueueMessageConsumer<GetResourcesRequestConsumer, GetResourcesRequest>();
builder.Services.AddQueueMessageConsumer<GetPipelinesRequestConsumer, GetPipelinesRequest>();
builder.Services.AddQueueMessageConsumer<PostResourceRequestConsumer, PostResourceRequest>();
builder.Services.AddQueueMessageConsumer<PostResourceFromPeerRequestConsumer, PostResourceFromPeerRequest>();
builder.Services.AddQueueMessageConsumer<PostOperatorRequestConsumer, PostOperatorRequest>();
builder.Services.AddQueueMessageConsumer<PostRepositoryRequestConsumer, PostRepositoryRequest>();
builder.Services.AddQueueMessageConsumer<PostPipelineRequestConsumer, PostPipelineRequest>();
builder.Services.AddQueueMessageConsumer<EditPipelineRequestConsumer, EditPipelineRequest>();

builder.Services.AddQueueMessageConsumer<GetResourceFilesRequestConsumer, GetResourceFilesRequest>();
builder.Services.AddQueueMessageConsumer<PostLoginRequestConsumer, PostLoginRequest>();
builder.Services.AddQueueMessageConsumer<PostRegistrationRequestConsumer, PostRegistrationRequest>();
builder.Services.AddQueueMessageConsumer<DeleteResourceFromRepoConsumer, DeleteResourceRequest>();
builder.Services.AddQueueMessageConsumer<GetPipelineExecutionsRequestConsumer, GetPipelineExecutionsRequest>();
builder.Services.AddQueueMessageConsumer<GetAllUsersRequestConsumer, GetAllUsersRequest>();
builder.Services.AddQueueMessageConsumer<PostUserRoleRequestConsumer, PostUserRoleRequest>();
builder.Services.AddQueueMessageConsumer<DeleteUserRequestConsumer, DeleteUserRequest>();
builder.Services.AddQueueMessageConsumer<DeleteRepositoryPipelineConsumer, DeletePipelineRequest>();

//Handshake
builder.Services.AddQueueMessageConsumer<CollabHandshakeRequestConsumer, CollabHandshakeRequest>();
builder.Services.AddQueueMessageConsumer<HandshakeRequestConsumer, HandshakeRequestMessage>();


//Pipeline Execution
builder.Services.AddQueueMessageConsumer<CreatePipelineExecutionRequestConsumer, CreatePipelineExecutionRequest>();
builder.Services.AddQueueMessageConsumer<TransferDataActionRequestConsumer, TransferDataActionRequest>();
builder.Services.AddQueueMessageConsumer<ExecuteOperatorActionRequestConsumer, ExecuteOperatorActionRequest>();
builder.Services.AddQueueMessageConsumer<PipelineStartCommandRequestConsumer, PipelineStartCommandRequest>();
builder.Services.AddQueueMessageConsumer<GetPipelineExecutionStatusRequestConsumer, GetPipelineExecutionStatusRequest>();




//SERVICE RESULTS
builder.Services.AddQueueMessageConsumer<GetOrgsFromRegistryResultConsumer, GetOrganizationsResultMessage>();
builder.Services.AddQueueMessageConsumer<GetReposFromRegistryResultConsumer, GetRepositoriesResultMessage>();
builder.Services.AddQueueMessageConsumer<GetResourcesFromRegistryResultConsumer, GetResourcesResultMessage>();
builder.Services.AddQueueMessageConsumer<PostResourceToRepoResultConsumer, PostResourceToRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<PostResourceToOperatorResultConsumer, PostInputResourceResultMessage>();
builder.Services.AddQueueMessageConsumer<PostResourceToRegistryResultConsumer, PostResourceToRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<PostRepoToRepoResultConsumer, PostRepoToRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<PostRepoToRegistryResultConsumer, PostRepoToRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<PostPipelineToRepoResultConsumer, PostPipelineToRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<EditPipelineInRepoResultConsumer, EditPipelineInRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelinesFromRepoResultConsumer, GetPipelinesFromRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<PostPipelineToRegistryResultConsumer, PostPipelineToRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<EditPipelineToRegistryResultConsumer, EditPipelineToRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelinesFromRegistryResultConsumer, GetPipelinesResultMessage>();
builder.Services.AddQueueMessageConsumer<GetResourceFilesFromRepoResultConsumer, GetResourceFilesFromRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<GetOperatorFilesFromRepoResultConsumer, GetOperatorFilesFromRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<GetResourceFilesFromOperatorResultConsumer, GetExecutionOutputResultMessage>();
builder.Services.AddQueueMessageConsumer<SendResourceToPeerResultConsumer, SendResourceToPeerResultMessage>();
builder.Services.AddQueueMessageConsumer<ExecuteOperatorResultConsumer, ExecuteOperatorResultMessage>();

builder.Services.AddQueueMessageConsumer<PostLoginResultConsumer,PostLoginResultMessage>();
builder.Services.AddQueueMessageConsumer<PostRegistrationResultConsumer,PostRegistrationResultMessage>();
builder.Services.AddQueueMessageConsumer<PostUserRoleResultConsumer,PostUserRoleResultMessage>();
builder.Services.AddQueueMessageConsumer<DeleteUserResultConsumer, DeleteUserResultMessage>();
builder.Services.AddQueueMessageConsumer<DeleteResourceFromRegistryConsumer, DeleteResourceFromRegistryResultMessage>();
builder.Services.AddQueueMessageConsumer<DeleteResourceFromRepoResultConsumer, DeleteResourceFromRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelineExecutionsFromRepoResultMessageConsumer, GetPipelineExecutionsFromRepoResultMessage>();
builder.Services.AddQueueMessageConsumer<GetAllUsersResultConsumer, GetAllUsersResultMessage>();

builder.Services.AddQueueMessageConsumer<DeleteRepositoryPipelineResultConsumer, DeleteRepositoryPipelineResultMessage>();
builder.Services.AddQueueMessageConsumer<DeleteRegistryPipelineResultConsumer, DeleteRegistryPipelineResultMessage>();


// Handshake
builder.Services.AddQueueMessageConsumer<RegistryUpdateAckConsumer, RegistryUpdateAckMessage>();
builder.Services.AddQueueMessageConsumer<HandshakeRequestResponseConsumer, HandshakeRequestResponseMessage>();
builder.Services.AddQueueMessageConsumer<RegistryUpdateConsumer, RegistryUpdateMessage>();
builder.Services.AddQueueMessageConsumer<ApplyRegistryUpdateResultConsumer, ApplyRegistryUpdateResult>();
builder.Services.AddQueueMessageConsumer<GetEntriesFromOrgResultConsumer, GetEntriesFromOrgResult>();


// Pipeline Execution
builder.Services.AddQueueMessageConsumer<ActionResultReceivedConsumer, ActionResultReceivedMessage>();
builder.Services.AddQueueMessageConsumer<CreatePipelineExecutionResultConsumer, CreatePipelineExecutionResultMessage>();
builder.Services.AddQueueMessageConsumer<CommandEnqueuedConsumer, CommandEnqueuedMessage>();
builder.Services.AddQueueMessageConsumer<GetPipelineExecutionStatusResultConsumer, GetPipelineExecutionStatusResultMessage>();

builder.Services.AddSingleton<IOrchestratorEngine, OrchestratorEngine>();
builder.Services.AddSingleton<IIdentityService, IdentityService>();

var app = builder.Build();

app.Services.GetService<IIdentityService>().GetIdentity();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");


app.UseAuthorization();

app.MapControllers();

app.Run();
