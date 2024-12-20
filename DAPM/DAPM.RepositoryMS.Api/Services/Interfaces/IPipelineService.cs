using DAPM.RepositoryMS.Api.Models.PostgreSQL;

/// <author>Tamas Drabos</author>

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IPipelineService
    {
        Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId);
        Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId);
        Task<IEnumerable<PipelineExecution>> GetPipelineExecutions(Guid pipelineId);
        Task<Tuple<Pipeline, PipelineExecution>> GetPipelineExecutionById(Guid executionId);
        Task<bool> DeletePipeline(Guid organizationId, Guid repositoryId, Guid pipelineId);
    }
}
