using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class PostLoginRequestConsumer : IQueueConsumer<PostLoginRequest>
    {
        IOrchestratorEngine _engine;
        public PostLoginRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(PostLoginRequest message)
        {
            _engine.StartPostLoginRequestProcess(message.TicketId, message.Username, message.Password);
            return Task.CompletedTask;
        }
    }
}
