using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;
/// <author>Ayat Al Rifai</author>

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class DeleteResourceFromRepoConsumer : IQueueConsumer<DeleteResourceFromRepoMessage>
    {
        private readonly ILogger<DeleteResourceFromRepoConsumer> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly IQueueProducer<DeleteResourceFromRepoResultMessage> _deleteResourceFromRepoResultProducer;

        public DeleteResourceFromRepoConsumer(
            ILogger<DeleteResourceFromRepoConsumer> logger,
            IRepositoryService repositoryService,
            IQueueProducer<DeleteResourceFromRepoResultMessage> deleteResourceFromRepoResultProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _deleteResourceFromRepoResultProducer = deleteResourceFromRepoResultProducer;
        }

        public async Task ConsumeAsync(DeleteResourceFromRepoMessage message)
        {
            _logger.LogInformation("DeleteResourceFromRepoMessage received");

            // Call the repository service to delete the resource
            var isDeleted = await _repositoryService.DeleteResource(message.OrganizationId, message.RepositoryId, message.ResourceId);

            if (isDeleted)
            {
                var deleteResourceFromRepoResult = new DeleteResourceFromRepoResultMessage
                {
                    ProcessId = message.ProcessId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Succeeded = true
                };

                _deleteResourceFromRepoResultProducer.PublishMessage(deleteResourceFromRepoResult);

                _logger.LogInformation("DeleteResourceFromRepoResultMessage produced");
            }
            else
            {
                _logger.LogError($"Failed to delete resource with ID: {message.ResourceId} from repository ID: {message.RepositoryId}");
            }

            return;
        }
    }
}
