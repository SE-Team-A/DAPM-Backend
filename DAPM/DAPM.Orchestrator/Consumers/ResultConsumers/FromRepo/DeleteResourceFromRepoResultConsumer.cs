using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
 // AYAT AL RIFAI
namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class DeleteResourceFromRepoResultConsumer : IQueueConsumer<DeleteResourceFromRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public DeleteResourceFromRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(DeleteResourceFromRepoResultMessage message)
        {
            var process = (DeleteResourceProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnDeleteResourcesFromRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
