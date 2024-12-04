using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

/// <author>Raihanullah Mehran</author>
namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class DeleteRegistryPipelineResultConsumer : IQueueConsumer<DeleteRegistryPipelineResultMessage>
    {
        private readonly IOrchestratorEngine _orchestratorEngine;
        public DeleteRegistryPipelineResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }
        public Task ConsumeAsync(DeleteRegistryPipelineResultMessage message)
        {
            DeletePipelineProcess process = (DeletePipelineProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnDeleteRegistryPipelineResult(message);

            return Task.CompletedTask;
        }
    }
}