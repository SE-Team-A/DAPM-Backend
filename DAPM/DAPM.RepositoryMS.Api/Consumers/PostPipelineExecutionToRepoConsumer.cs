using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.RepositoryMS.Api.Consumers
{
    public class PostPipelineExecutionToRepoConsumer : IQueueConsumer<PostPipelineExecutionToRepoMessage>
    {
        private ILogger<PostPipelineExecutionToRepoConsumer> _logger;

        private IRepositoryService _repositoryService;

        IQueueProducer<CreatePipelineExecutionResultMessage> _postPipelineExecutionToRepoResultProducer;

        public PostPipelineExecutionToRepoConsumer(ILogger<PostPipelineExecutionToRepoConsumer> logger,
            IRepositoryService repositoryService,
            IQueueProducer<PostPipelineExecutionToRepoResultMessage> postPipelineExecutionToRepoResultProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _postPipelineExecutionToRepoResultProducer = postPipelineExecutionToRepoResultProducer;
        }

         public async Task ConsumeAsync(PostPipelineExecutionToRepoMessage message)
        {
            _logger.LogInformation("PostPipelineExecutionToRepoMessage received");

            Models.PostgreSQL.PipelineExecution pipelineExecution = await _repositoryService.CreateNewPipeline(message.RepositoryId, message.Name, message.Pipeline);

            if (pipelineExecution != null)
            {
                var pipelineExecutionStatusDTO = new PipelineExecutionStatusDTO()
                {
                    Id = pipeline.Id,
                    RepositoryId = pipeline.RepositoryId,
                    Name = pipeline.Name,
                    Pipeline = JsonConvert.DeserializeObject<RabbitMQLibrary.Models.Pipeline>(pipeline.PipelineJson)
                };

                var resultMessage = new PostPipelineExecutionToRepoResultMessage
                {
                    ProcessId = message.ProcessId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    Message = "Item created successfully",
                    Succeeded = true,
                    Pipeline = pipelineDTO
                };

                _postPipelineToRepoResultProducer.PublishMessage(resultMessage);

                _logger.LogInformation("PostPipelineToRepoResultMessage produced");

            }
            else
            {
                _logger.LogInformation("There was an error creating the new pipeline");
            }

            return;
        }
}