﻿using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;

namespace DAPM.Orchestrator.Consumers.StartProcessConsumers
{
    public class DeleteResourceFromRepoConsumer: IQueueConsumer<DeleteResourceRequest>
    {
        IOrchestratorEngine _engine;
        public DeleteResourceFromRepoConsumer(IOrchestratorEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(DeleteResourceRequest message)
        {
            _engine.StartDeleteResourceProcess(message.TicketId, message.OrganizationId, message.RepositoryId, message.ResourceId);

            return Task.CompletedTask;
        }
    }
}