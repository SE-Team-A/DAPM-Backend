using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

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
