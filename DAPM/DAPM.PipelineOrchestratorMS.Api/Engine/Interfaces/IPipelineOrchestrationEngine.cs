using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Models;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces
{
    public interface IPipelineOrchestrationEngine
    {
        public Guid CreatePipelineExecutionInstance(PipelineDTO pipeline, Guid processId);
        public void ExecutePipelineStartCommand(Guid processId, Guid executionId);
        public void OnPipelineExecutionRetrieved(PipelineDTO pipeline, RabbitMQLibrary.Models.PipelineExecution ex);
        public PipelineExecutionStatus GetPipelineExecutionStatus(Guid executionId);
        public void ProcessActionResult(ActionResultDTO actionResultDto);
    }
}
