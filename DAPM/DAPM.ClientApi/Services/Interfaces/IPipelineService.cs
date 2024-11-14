using DAPM.ClientApi.Models.DTOs;
using RabbitMQLibrary.Models;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IPipelineService
    {
        public Guid GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId);
        public Guid GetPipelinesForRepository(Guid organizationId, Guid repositoryId);
        public Guid CreatePipelineExecution(Guid organizationId, Guid repositoryId, Guid pipelineId);
        public Guid GetPipelineExecutionByPipelineId(Guid organizationId, Guid repositoryId, Guid pipelineId);
        public Guid PostStartCommand(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId);
        public Guid GetExecutionStatus(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId);
    }
}
