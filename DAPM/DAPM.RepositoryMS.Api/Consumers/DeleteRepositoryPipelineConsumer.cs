using DAPM.RepositoryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class DeleteRepositoryPipelineConsumer : IQueueConsumer<DeleteRepositoryPipelineMessage>
    {
        private readonly ILogger<DeleteResourceFromRepoConsumer> _logger;
        private readonly IPipelineService _pipelineService;
        private readonly IQueueProducer<DeleteRepositoryPipelineResultMessage> _deleteRepositoryPipelineResultProducer;

        public DeleteRepositoryPipelineConsumer(
            ILogger<DeleteResourceFromRepoConsumer> logger,
            IPipelineService pipelineService,
            IQueueProducer<DeleteRepositoryPipelineResultMessage> deleteRepositoryPipelineResultProducer)
        {
            _logger = logger;
            _pipelineService = pipelineService;
            _deleteRepositoryPipelineResultProducer = deleteRepositoryPipelineResultProducer;
        }

        public async Task ConsumeAsync(DeleteRepositoryPipelineMessage message)
        {
            _logger.LogInformation("DeleteRepositoryPipelineMessage received");

            var isDeleted = await _pipelineService.DeletePipeline(
                organizationId: message.OrganizationId,
                repositoryId: message.RepositoryId,
                pipelineId: message.PipelineId);

            if (isDeleted)
            {
                var deleteRepositoryPiplineResult = new DeleteRepositoryPipelineResultMessage
                {
                    ProcessId = message.ProcessId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Succeeded = true
                };

                _deleteRepositoryPipelineResultProducer.PublishMessage(deleteRepositoryPiplineResult);
                _logger.LogInformation("DeleteRepositoryPiplineResultMessage produced");

                return;
            }

            _logger.LogError($"Failed to delete resource with ID: {message.PipelineId} from repository ID: {message.RepositoryId}");
            return;
        }
    }
}