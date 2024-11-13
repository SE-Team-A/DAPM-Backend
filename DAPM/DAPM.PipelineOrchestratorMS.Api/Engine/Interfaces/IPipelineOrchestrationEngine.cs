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
        public void ExecutePipelineStartCommand(Guid executionId);
        public PipelineExecutionStatus GetPipelineExecutionStatus(Guid executionId);
        public void ProcessActionResult(ActionResultDTO actionResultDto);
    }
}
