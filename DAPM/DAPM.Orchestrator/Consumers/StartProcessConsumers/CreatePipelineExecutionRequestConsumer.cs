using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class CreatePipelineExecutionRequestConsumer : IQueueConsumer<CreatePipelineExecutionRequest>
    {
        IOrchestratorEngine _engine;

        public CreatePipelineExecutionRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(CreatePipelineExecutionRequest message)
        {
            _engine.StartCreatePipelineExecutionProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.PipelineExecution, message.Name);
            return Task.CompletedTask;
        }
    }
}
