using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;
/// <author>Ayat Al Rifai</author>

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class DeleteResourceFromRegistryConsumer: IQueueConsumer<DeleteResourceFromRegistryMessage>
    {
        private ILogger<DeleteResourceFromRegistryConsumer> _logger;
        private IQueueProducer<DeleteResourceFromRegistryResultMessage> _getDeleteResourcesResultQueueProducer;
        private IResourceService _resourceService;
        public DeleteResourceFromRegistryConsumer(ILogger<DeleteResourceFromRegistryConsumer> logger,
            IQueueProducer<DeleteResourceFromRegistryResultMessage> getDeleteResourcesResultQueueProducer,
            IRepositoryService repositoryService,
            IResourceService resourceService)
        {
            _logger = logger;
            _getDeleteResourcesResultQueueProducer = getDeleteResourcesResultQueueProducer;
            _resourceService = resourceService;
        }

        public async Task ConsumeAsync(DeleteResourceFromRegistryMessage message)
        {
            _logger.LogInformation("GetDeleteResourcesMessage received");
                await _resourceService.DeleteResource(message.OrganizationId, message.RepositoryId, message.ResourceId);

           var resultMessage = new DeleteResourceFromRegistryResultMessage
            {
                MessageId = message.MessageId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId,
                organizationId = message.OrganizationId,
                repositoryId = message.RepositoryId,
                ResourceId = message.ResourceId,
            };

            _getDeleteResourcesResultQueueProducer.PublishMessage(resultMessage);
            

            return;
        }
    }
}
