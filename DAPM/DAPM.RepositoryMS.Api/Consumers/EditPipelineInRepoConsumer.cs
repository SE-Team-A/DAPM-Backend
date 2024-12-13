using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.RepositoryMS.Api.Consumers
{
    public class EditPipelineInRepoConsumer : IQueueConsumer<EditPipelineInRepoMessage>
    {
        private ILogger<EditPipelineInRepoConsumer> _logger;
        private IRepositoryService _repositoryService;
        IQueueProducer<EditPipelineInRepoResultMessage> _editPipelineInRepoResultProducer;

        public EditPipelineInRepoConsumer(ILogger<EditPipelineInRepoConsumer> logger,
            IRepositoryService repositoryService,
            IQueueProducer<EditPipelineInRepoResultMessage> editPipelineInRepoResultProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _editPipelineInRepoResultProducer = editPipelineInRepoResultProducer;
        }

        public async Task ConsumeAsync(EditPipelineInRepoMessage message)
        {
            _logger.LogInformation("EditPipelineInRepoMessage received");

            Models.PostgreSQL.Pipeline pipeline = await _repositoryService.EditPipeline(message.RepositoryId, message.Name, message.Pipeline, message.PipelineId);

            if (pipeline != null)
            {
                var pipelineDTO = new PipelineDTO()
                {
                    Id = pipeline.Id,
                    RepositoryId = pipeline.RepositoryId,
                    Name = pipeline.Name,
                    Pipeline = JsonConvert.DeserializeObject<RabbitMQLibrary.Models.Pipeline>(pipeline.PipelineJson)
                };

                var resultMessage = new EditPipelineInRepoResultMessage
                {
                    ProcessId = message.ProcessId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Message = "Item created successfully",
                    Succeeded = true,
                    Pipeline = pipelineDTO
                };

                _editPipelineInRepoResultProducer.PublishMessage(resultMessage);

                _logger.LogInformation("EditPipelineInRepoResultMessage produced");

            }
            else
            {
                _logger.LogInformation("There was an error editinng the new pipeline");
            }

            return;
        }
    }
}
