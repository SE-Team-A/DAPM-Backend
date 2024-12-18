using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostUserRoleRequestConsumer : IQueueConsumer<PostUserRoleRequest>
    {
        IOrchestratorEngine _engine;
        ILogger<PostUserRoleRequestConsumer> _logger;
        public PostUserRoleRequestConsumer(IOrchestratorEngine engine, ILogger<PostUserRoleRequestConsumer> logger)
        {
            _engine = engine;
            _logger = logger;
        }
        public Task ConsumeAsync(PostUserRoleRequest message)
        {
            _logger.LogInformation($"Set role process enqueued. Token: {message.RequestToken}, UserId: {message.UserId}, RoleName: {message.RoleName}");
            _engine.StartPostUserRoleProcess(message.TicketId, message.RequestToken, message.UserId, message.RoleName);
            return Task.CompletedTask;
        }
    }
}
