using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class EditPipelineInRepoResultConsumer : IQueueConsumer<EditPipelineInRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public EditPipelineInRepoResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(EditPipelineInRepoResultMessage message)
        {
            EditPipelineProcess process = (EditPipelineProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnEditPipelineToRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
