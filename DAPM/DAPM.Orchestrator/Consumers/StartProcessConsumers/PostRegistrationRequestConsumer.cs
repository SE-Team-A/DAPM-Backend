using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostRegistrationRequestConsumer : IQueueConsumer<PostRegistrationRequest>
    {
        IOrchestratorEngine _engine;
        public PostRegistrationRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(PostRegistrationRequest message)
        {
            _engine.StartPostRegistrationRequestProcess(message.TicketId, message.Username, message.Password, message.Name, message.Role);
            return Task.CompletedTask;
        }
    }
}
