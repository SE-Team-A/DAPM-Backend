using Microsoft.Extensions.DependencyInjection;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

/// <author>Tamas Drabos</author>

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class GetPipelineExecutionsRequestConsumer : IQueueConsumer<GetPipelineExecutionsRequest>
    {
        IOrchestratorEngine _engine;

        public GetPipelineExecutionsRequestConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(GetPipelineExecutionsRequest message)
        {
            _engine.StartGetPipelineExecutionsProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.PipelineId);
            return Task.CompletedTask;
        }
        
    }
}
