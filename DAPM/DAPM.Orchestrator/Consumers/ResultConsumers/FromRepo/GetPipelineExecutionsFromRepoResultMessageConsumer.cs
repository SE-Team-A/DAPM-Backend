using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class GetPipelineExecutionsFromRepoResultMessageConsumer: IQueueConsumer<GetPipelineExecutionsFromRepoResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public GetPipelineExecutionsFromRepoResultMessageConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(GetPipelineExecutionsFromRepoResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnGetPipelineExecutionsFromRepoResult(message);

            return Task.CompletedTask;
        }
    }
}
