using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetAllUsersRequestConsumer : IQueueConsumer<GetAllUsersRequest>
    {
        IOrchestratorEngine _engine;
        public GetAllUsersRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(GetAllUsersRequest message)
        {
            _engine.StartGetAllUsersProcess(message.TicketId, message.Token);
            return Task.CompletedTask;
        }
    }
}