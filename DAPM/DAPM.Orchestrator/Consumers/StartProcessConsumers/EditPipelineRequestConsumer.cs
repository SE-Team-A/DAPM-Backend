using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class EditPipelineRequestConsumer : IQueueConsumer<EditPipelineRequest>
    {
        
        private IOrchestratorEngine _engine;

        public EditPipelineRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(EditPipelineRequest message)
        {
            _engine.StartEditPipelineProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.Pipeline, message.Name, message.PipelineId);
            return Task.CompletedTask;
        }
    }
}
