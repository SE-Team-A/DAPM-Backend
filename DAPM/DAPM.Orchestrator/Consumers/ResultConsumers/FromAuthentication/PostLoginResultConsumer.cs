using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRegistry
{
    public class PostLoginResultConsumer : IQueueConsumer<PostLoginResultMessage>
    {
        private IOrchestratorEngine _orchestratorEngine;

        public PostLoginResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(PostLoginResultMessage message)
        {
            OrchestratorProcess process = _orchestratorEngine.GetProcess(message.ProcessId);
            process.OnPostLoginResult(message);

            return Task.CompletedTask;
        }
    }
}
