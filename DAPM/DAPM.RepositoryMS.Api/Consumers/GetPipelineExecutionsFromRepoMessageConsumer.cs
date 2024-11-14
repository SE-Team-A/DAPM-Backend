using DAPM.RepositoryMS.Api.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

namespace DAPM.RepositoryMS.Api.Consumers
{
    public class GetPipelineExecutionsFromRepoMessageConsumer: IQueueConsumer<GetPipelineExecutionsFromRepoMessage>
    {
        private ILogger<GetPipelineExecutionsFromRepoMessage> _logger;
        private IPipelineService _pipelineService;
        private IQueueProducer<GetPipelineExecutionsFromRepoResultMessage> _getPipelineExecutionsFromRepoResultProducer;

        public GetPipelineExecutionsFromRepoMessageConsumer(ILogger<GetPipelineExecutionsFromRepoMessage> logger, IPipelineService pipelineService,
             IQueueProducer<GetPipelineExecutionsFromRepoResultMessage> getPipelineExecutionsFromRepoResultProducer)
        {
            _logger = logger;
            _pipelineService = pipelineService;
            _getPipelineExecutionsFromRepoResultProducer = getPipelineExecutionsFromRepoResultProducer;
        }

        public async Task ConsumeAsync(GetPipelineExecutionsFromRepoMessage message)
        {
            _logger.LogInformation("GetPipelineExecutionsFromRepoMessage received");
            var executions = Enumerable.Empty<Models.PostgreSQL.PipelineExecution>();

            
            executions = await _pipelineService.GetPipelineExecutions(message.RepositoryId, message.PipelineId);
            

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

            return;
        }
    }
}
