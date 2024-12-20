using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class EditPipelineToRegistryConsumer : IQueueConsumer<EditPipelineToRegistryMessage>
    {
        private ILogger<EditPipelineToRegistryConsumer> _logger;
        private IRepositoryService _repositoryService;
        private IQueueProducer<EditPipelineToRegistryResultMessage> _editPipelineToRegistryResultProducer;
        public EditPipelineToRegistryConsumer(ILogger<EditPipelineToRegistryConsumer> logger,
            IQueueProducer<EditPipelineToRegistryResultMessage> editPipelineToRegistryResultProducer,
            IRepositoryService repositoryService)
        {
            _logger = logger;
            _editPipelineToRegistryResultProducer = editPipelineToRegistryResultProducer;
            _repositoryService = repositoryService;
        }
        public async Task ConsumeAsync(EditPipelineToRegistryMessage message)
        {
            _logger.LogInformation("PostPipelineToRegistryMessage received");

            var pipelineDto = message.Pipeline;
            if (pipelineDto != null)
            {
                var createdPipeline = _repositoryService.EditPipelineToRepository(pipelineDto.OrganizationId, pipelineDto.RepositoryId, pipelineDto, message.PipelineId);
                if (createdPipeline != null)
                {
                    var resultMessage = new EditPipelineToRegistryResultMessage
                    {
                        ProcessId = message.ProcessId,
                        TimeToLive = TimeSpan.FromMinutes(1),
                        Message = "Item created successfully",
                        Succeeded = true,
                        Pipeline = pipelineDto
                    };

                    _editPipelineToRegistryResultProducer.PublishMessage(resultMessage);
                    _logger.LogInformation("EditPipelineToRegistryResultMessage published");
                }
            }

            return;
        }
    }
}
