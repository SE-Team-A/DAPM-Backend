using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

/// <author>Vladyslav Synytskyi</author>

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class DeleteUserRequestConsumer : IQueueConsumer<DeleteUserRequest>
    {
        IOrchestratorEngine _engine;
        ILogger<DeleteUserRequestConsumer> _logger;
        public DeleteUserRequestConsumer(IOrchestratorEngine engine, ILogger<DeleteUserRequestConsumer> logger)
        {
            _engine = engine;
            _logger = logger;
        }
        public Task ConsumeAsync(DeleteUserRequest message)
        {
            _logger.LogInformation($"Delete user process enqueued. Token: {message.RequestToken}, UserId: {message.UserId}");
            _engine.StartDeleteUserProcess(message.TicketId, message.RequestToken, message.UserId);
            return Task.CompletedTask;
        }
    }
}