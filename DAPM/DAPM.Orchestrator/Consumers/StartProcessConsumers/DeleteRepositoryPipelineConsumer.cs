using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

/// <author>Raihanullah Mehran</author>
namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class DeleteRepositoryPipelineConsumer : IQueueConsumer<DeletePipelineRequest>
    {
        private readonly IOrchestratorEngine _orchestratorEngine;
        public DeleteRepositoryPipelineConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(DeletePipelineRequest message)
        {
            _orchestratorEngine.StartDeletePipelineProcess(
                messageTicketId: message.TicketId,
                messageOrganizationId: message.OrganizationId,
                messageRepositoryId: message.RepositoryId,
                messagePipelineId: message.PipelineId
            );

            return Task.CompletedTask;
        }
    }
}