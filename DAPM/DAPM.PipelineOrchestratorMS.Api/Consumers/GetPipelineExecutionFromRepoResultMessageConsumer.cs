using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.PipelineOrchestratorMS.Api.Consumers
{
    public class GetPipelineExecutionFromRepoResultMessageConsumer : IQueueConsumer<GetPipelineExecutionFromRepoResultMessage>
    {
        private IPipelineOrchestrationEngine _engine;
        private ILogger<GetPipelineExecutionFromRepoResultMessageConsumer> _logger;

        public GetPipelineExecutionFromRepoResultMessageConsumer(IPipelineOrchestrationEngine engine, ILogger<GetPipelineExecutionFromRepoResultMessageConsumer> logger)
        {
            _engine = engine;
            _logger = logger;
        }

        public Task ConsumeAsync(GetPipelineExecutionFromRepoResultMessage message)
        {
            _logger.LogInformation("GetPipelineExecutionFromRepoResultMessage received");
            _engine.OnPipelineExecutionRetrieved(message.Pipeline, message.Execution);
            return Task.CompletedTask;
        }
    }
}
