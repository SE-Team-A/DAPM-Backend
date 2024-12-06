using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

/// <author>Raihanullah Mehran</author>
namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class DeleteRegistryPipelineConsumer : IQueueConsumer<DeleteRegistryPipelineMessage>
    {
        private readonly ILogger<DeleteRegistryPipelineConsumer> _logger;
        private readonly IQueueProducer<DeleteRegistryPipelineResultMessage> _getDeleteRegistryPipelineResultQueueProducer;
        private readonly IPipelineService _pipelineService;

        public DeleteRegistryPipelineConsumer(
            ILogger<DeleteRegistryPipelineConsumer> logger,
            IQueueProducer<DeleteRegistryPipelineResultMessage> getDeleteRegistryPipelineResultQueueProducer,
            IPipelineService pipelineService)
        {
            _logger = logger;
            _getDeleteRegistryPipelineResultQueueProducer = getDeleteRegistryPipelineResultQueueProducer;
            _pipelineService = pipelineService;
        }
        public async Task ConsumeAsync(DeleteRegistryPipelineMessage message)
        {
            _logger.LogInformation("DeleteRegistryPipelineMessage received");

            await _pipelineService.DeletePipeline(organizationId: message.OrganizationId, repositoryId: message.RepositoryId, pipelineId: message.PipelineId);

            var resultMessage = new DeleteRegistryPipelineResultMessage
            {
                MessageId = message.MessageId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId,
                OrganizationId = message.OrganizationId,
                RepositoryId = message.RepositoryId,
                PipelineId = message.PipelineId
            };

            _getDeleteRegistryPipelineResultQueueProducer.PublishMessage(resultMessage);

            return;
        }
    }
}