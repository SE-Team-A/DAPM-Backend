using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class EditPipelineToRegistryResultConsumer : IQueueConsumer<EditPipelineToRegistryResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public EditPipelineToRegistryResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(EditPipelineToRegistryResultMessage message)
        {
            EditPipelineProcess process = (EditPipelineProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnEditPipelineToRegistryResult(message);

            return Task.CompletedTask;
        }
    }
}
