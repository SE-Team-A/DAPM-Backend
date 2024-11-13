using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

namespace DAPM.RepositoryMS.Api.Services
{
    public class PipelineService : IPipelineService
    {
        private IPipelineRepository _pipelineRepository;
        public PipelineService(IPipelineRepository pipelineRepository) 
        {
            _pipelineRepository = pipelineRepository;
        }
        public Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId)
        {
            return _pipelineRepository.GetPipelineById(repositoryId, pipelineId);
        }

        public Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId)
        {
            return _pipelineRepository.GetPipelines(repositoryId);
        }

        public Task<IEnumerable<PipelineExecution>> GetPipelineExecutions(Guid repositoryId, Guid pipelineId)
        {
            return _pipelineRepository.GetPipelineExecutions(repositoryId, pipelineId);
        }
    }
}
