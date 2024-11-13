using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class DeleteUserResultConsumer : IQueueConsumer<DeleteUserResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public DeleteUserResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(DeleteUserResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnDeleteUserResult(message);
            return Task.CompletedTask;
        }
    }
}