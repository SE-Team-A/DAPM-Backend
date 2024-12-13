using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using DAPM.RepositoryMS.Api.Services.Interfaces;

/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>

namespace DAPM.RepositoryMS.Api.Services
{
    public class PipelineService : IPipelineService
    {
        ILogger<PipelineService> _logger;
        private IPipelineRepository _pipelineRepository;
        public PipelineService(ILogger<PipelineService> logger, IPipelineRepository pipelineRepository)
        {
            _logger = logger;
            _pipelineRepository = pipelineRepository;
        }

        public async Task<bool> DeletePipeline(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            _logger.LogInformation($"Deleting pipeline with ID: {pipelineId} from repository ID: {repositoryId}");

            var pipeline = await _pipelineRepository.GetPipelineById(repositoryId, pipelineId);

            if (pipeline == null)
            {
                _logger.LogWarning($"Pipeline with ID: {pipeline} not found in repository ID: {repositoryId}");
                return false;
            }

            bool pipelineDelete = await _pipelineRepository.DeletePipeline(organizationId: organizationId, repositoryId: repositoryId, pipelineId: pipelineId);

            if (pipelineDelete)
            {
                _logger.LogInformation($"Successfully deleted pipeline with ID: {pipelineId} from repository ID: {repositoryId}");
                return true;
            }

            _logger.LogError($"Failed to delete the pipeline with ID: {pipelineId}");
            return false;
        }

        public Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId)
        {
            return _pipelineRepository.GetPipelineById(repositoryId, pipelineId);
        }

        public Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId)
        {
            return _pipelineRepository.GetPipelines(repositoryId);
        }

        public Task<IEnumerable<PipelineExecution>> GetPipelineExecutions(Guid pipelineId)
        {
            return _pipelineRepository.GetPipelineExecutions(pipelineId);
        }

        public Task<Tuple<Pipeline, PipelineExecution>> GetPipelineExecutionById(Guid executionId)
        {
            return _pipelineRepository.GetPipelineExecutionById(executionId);
        }

    }
}
