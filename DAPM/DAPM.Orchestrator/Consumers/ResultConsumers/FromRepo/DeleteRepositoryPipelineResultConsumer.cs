using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAPM.Orchestrator.Processes;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

/// <author>Raihanullah Mehran</author>
namespace DAPM.Orchestrator.Consumers.ResultConsumers.FromRepo
{
    public class DeleteRepositoryPipelineResultConsumer : IQueueConsumer<DeleteRepositoryPipelineResultMessage>
    {
        private readonly IOrchestratorEngine _orchestratorEngine;
        public DeleteRepositoryPipelineResultConsumer(IOrchestratorEngine orchestratorEngine)
        {
            _orchestratorEngine = orchestratorEngine;
        }

        public Task ConsumeAsync(DeleteRepositoryPipelineResultMessage message)
        {
            var process = (DeletePipelineProcess)_orchestratorEngine.GetProcess(message.ProcessId);
            process.OnDeleteRepositoryPipelineResult(message);

            return Task.CompletedTask;
        }
    }
}