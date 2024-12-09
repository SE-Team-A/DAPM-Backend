using DAPM.RepositoryMS.Api.Models.PostgreSQL;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IPipelineRepository
    {
        Task<Pipeline> AddPipeline(Pipeline pipeline);

        Task<PipelineExecution> AddPipelineExecution(PipelineExecution pipelineExecution);
        Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId);
        Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId);
        Task<IEnumerable<PipelineExecution>> GetPipelineExecutions(Guid pipelineId);
        Task<Tuple<Pipeline, PipelineExecution>> GetPipelineExecutionById(Guid executionId);
        Task<Pipeline> EditPipeline(Pipeline pipeline, Guid pipelindId);

        Task<bool> DeletePipeline(Guid organizationId, Guid repositoryId, Guid pipelineId);

        Task<bool> UpdatePipelineExecutionStatus(Guid executionId, string status);
    }
}
