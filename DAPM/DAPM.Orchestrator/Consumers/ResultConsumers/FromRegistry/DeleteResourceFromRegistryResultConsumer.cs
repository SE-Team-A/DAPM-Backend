using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;


namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class DeleteResourceFromRegistryConsumer : IQueueConsumer<DeleteResourceFromRegistryResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public DeleteResourceFromRegistryConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(DeleteResourceFromRegistryResultMessage message)
        {
            DeleteResourceProcess process = (DeleteResourceProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnDeleteResourceFromRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
