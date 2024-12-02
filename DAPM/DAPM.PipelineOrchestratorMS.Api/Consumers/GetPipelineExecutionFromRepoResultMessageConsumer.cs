using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;

namespace DAPM.PipelineOrchestratorMS.Api.Consumers
{
    public class GetPipelineExecutionFromRepoResultMessageConsumer : IQueueConsumer<GetPipelineExecutionFromRepoResultMessage>
    {
        private IPipelineOrchestrationEngine _engine;

        public GetPipelineExecutionFromRepoResultMessageConsumer(IPipelineOrchestrationEngine engine)
        {
            _engine = engine;
        }

        public Task ConsumeAsync(GetPipelineExecutionFromRepoResultMessage message)
        {
            _engine.OnPipelineExecutionRetrieved(message.Pipeline, message.Execution);
            return Task.CompletedTask;
        }
    }
}
