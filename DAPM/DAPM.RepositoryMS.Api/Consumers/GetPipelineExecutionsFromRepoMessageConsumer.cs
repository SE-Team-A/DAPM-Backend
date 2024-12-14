using DAPM.RepositoryMS.Api.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class GetPipelineExecutionsFromRepoMessageConsumer: IQueueConsumer<GetPipelineExecutionsFromRepoMessage>
    {
        private ILogger<GetPipelineExecutionsFromRepoMessage> _logger;
        private IPipelineService _pipelineService;
        private IQueueProducer<GetPipelineExecutionsFromRepoResultMessage> _getPipelineExecutionsFromRepoResultProducer;
        private IQueueProducer<GetPipelineExecutionFromRepoResultMessage> _getPipelineExecutionFromRepoResultProducer;

        public GetPipelineExecutionsFromRepoMessageConsumer(ILogger<GetPipelineExecutionsFromRepoMessage> logger, IPipelineService pipelineService,
             IQueueProducer<GetPipelineExecutionsFromRepoResultMessage> getPipelineExecutionsFromRepoResultProducer, 
             IQueueProducer<GetPipelineExecutionFromRepoResultMessage> getPipelineExecutionFromRepoResultProducer)
        {
            _logger = logger;
            _pipelineService = pipelineService;
            _getPipelineExecutionsFromRepoResultProducer = getPipelineExecutionsFromRepoResultProducer;
            _getPipelineExecutionFromRepoResultProducer = getPipelineExecutionFromRepoResultProducer;
        }

        public async Task ConsumeAsync(GetPipelineExecutionsFromRepoMessage message)
        {
            _logger.LogInformation("GetPipelineExecutionsFromRepoMessage received");

            if (message.ExecutionId != null)
            {
                Guid executionId = message.ExecutionId.Value;
                var values = await _pipelineService.GetPipelineExecutionById(executionId);
                var pipeline = values.Item1;

                var executionDto = new RabbitMQLibrary.Models.PipelineExecution()
                {
                    ExecutionId = values.Item2.Id,
                    PipelineId = values.Item2.PipelineId,
                    State = values.Item2.Status
                };

                var pipelineDto = new PipelineDTO()
                {
                    RepositoryId = pipeline.RepositoryId,
                    Id = pipeline.Id,
                    Name = pipeline.Name,
                    Pipeline = JsonConvert.DeserializeObject<RabbitMQLibrary.Models.Pipeline>(pipeline.PipelineJson)
                };

                var resultMessage = new GetPipelineExecutionFromRepoResultMessage()
                {
                    Execution = executionDto,
                    Pipeline = pipelineDto,
                    TimeToLive = TimeSpan.FromMinutes(1),
                };

                _getPipelineExecutionFromRepoResultProducer.PublishMessage(resultMessage);
                _logger.LogInformation("GetPipelineExecutionFromRepoResultMessage Enqueued");

                var updateResult = await _pipelineService.UpdatePipelineExecutionStatus(executionId, executionDto.State);
                if (updateResult)
                {
                    _logger.LogInformation($"Successfully updated status of pipeline execution with ID: {executionId} to {executionDto.State}");
                }
                else
                {
                    _logger.LogError($"Failed to update status of pipeline execution with ID: {executionId}");
                }  
            } else {
                var executions = Enumerable.Empty<Models.PostgreSQL.PipelineExecution>();
                executions = await _pipelineService.GetPipelineExecutions(message.PipelineId);
            

                var pipelineExecutionDTOs = Enumerable.Empty<RabbitMQLibrary.Models.PipelineExecution>();

                foreach (var e in executions)
                {
                    var dto = new RabbitMQLibrary.Models.PipelineExecution()
                    {
                        ExecutionId = e.Id,
                        PipelineId = e.PipelineId,
                        State = e.Status
                    };

                    pipelineExecutionDTOs = pipelineExecutionDTOs.Append(dto);
                }


                var resultMessage = new GetPipelineExecutionsFromRepoResultMessage()
                {
                    TimeToLive = TimeSpan.FromMinutes(1),
                    ProcessId = message.ProcessId,
                    PipelineExecutions = pipelineExecutionDTOs
                };

                _getPipelineExecutionsFromRepoResultProducer.PublishMessage(resultMessage);

                _logger.LogInformation("GetPipelineExecutionsFromRepoResultMessage Enqueued");

            }

            return;
        }
    }
}
