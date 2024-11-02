using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IPipelineRepository
    {
        Task<Pipeline> AddPipeline(Pipeline pipeline);

        Task<PipelineExecution> AddPipelineExecution(PipelineExecution pipelineExecution);
        Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId);
        Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId);
    }
}
