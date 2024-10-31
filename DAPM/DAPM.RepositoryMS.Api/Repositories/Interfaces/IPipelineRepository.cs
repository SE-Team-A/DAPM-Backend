using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Repositories.Interfaces
{
    public interface IPipelineRepository
    {
        Task<Pipeline> AddPipeline(Pipeline pipeline);
        Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId);
        Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId);
        Task<Pipeline> EditPipeline(Pipeline pipeline, Guid pipelindId);

    }
}
