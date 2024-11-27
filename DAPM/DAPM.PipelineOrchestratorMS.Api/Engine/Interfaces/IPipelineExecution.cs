using DAPM.PipelineOrchestratorMS.Api.Models;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces
{
    public interface IPipelineExecution
    {
        public void StartExecution(Guid executionId);
        public PipelineExecutionStatus GetStatus();
        public void ProcessActionResult(ActionResultDTO actionResultDto);
    }
}
