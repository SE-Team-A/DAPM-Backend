using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class PostUserRoleResultConsumer : IQueueConsumer<PostUserRoleResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostUserRoleResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostUserRoleResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnPostUserRoleResult(message);
            return Task.CompletedTask;
        }
    }
}
