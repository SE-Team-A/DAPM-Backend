using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;
using PipelineExecution = RabbitMQLibrary.Models.PipelineExecution;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.RepositoryMS.Api.Consumers
{
    public class PostPipelineExecutionToRepoConsumer : IQueueConsumer<PostPipelineExecutionToRepoMessage>
    {
        private ILogger<PostPipelineExecutionToRepoConsumer> _logger;

        private IRepositoryService _repositoryService;

        IQueueProducer<CreatePipelineExecutionResultMessage> _queueProducer;

        public PostPipelineExecutionToRepoConsumer(ILogger<PostPipelineExecutionToRepoConsumer> logger,
            IRepositoryService repositoryService,
            IQueueProducer<CreatePipelineExecutionResultMessage> queueProducer)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _queueProducer = queueProducer;
        }

        public async Task ConsumeAsync(PostPipelineExecutionToRepoMessage message)
        {
            _logger.LogInformation("PostPipelineExecutionToRepoMessage received");

            var pipelineExecution =
                await _repositoryService.CreateNewPipelineExecution(message.RepositoryId, message.PipelineId,
                    "Not Started");

            if (pipelineExecution != null)
            {
                var newExecution = new PipelineExecution()
                {
                    ExecutionId = pipelineExecution.Id,
                    CreatedAt = DateTime.Now,
                    PipelineId = pipelineExecution.PipelineId,
                    State = "Not Started"
                };

                var resultMessage = new CreatePipelineExecutionResultMessage
                {
                    ProcessId = message.ProcessId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    PipelineExecution = newExecution,
                    Succeeded = true,
                };

                _queueProducer.PublishMessage(resultMessage);

                _logger.LogInformation("CreatePipelineExecutionResultMessage produced");

            }
            else
            {
                _logger.LogInformation("There was an error creating the new pipeline");
            }

            return;
        }
    }
}