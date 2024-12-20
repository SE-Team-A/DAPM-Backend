using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
using DAPM.ResourceRegistryMS.Api.Services.Interfaces;
/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ResourceRegistryMS.Api.Services
{
    public class PipelineService : IPipelineService
    {
        private IPipelineRepository _pipelineRepository;

        public PipelineService(IPipelineRepository pipelineRepository)
        {
            _pipelineRepository = pipelineRepository;
        }

        public async Task<bool> DeletePipeline(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            return await _pipelineRepository.DeletePipeline(
                organizationId: organizationId,
                repositoryId: repositoryId,
                pipelineId: pipelineId
            );
        }

        public async Task<Pipeline> GetPipelineById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            return await _pipelineRepository.GetPipelineById(organizationId, repositoryId, resourceId);
        }
    }
}
