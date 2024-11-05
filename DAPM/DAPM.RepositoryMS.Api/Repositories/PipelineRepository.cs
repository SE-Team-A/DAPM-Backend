using DAPM.RepositoryMS.Api.Data;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAPM.RepositoryMS.Api.Repositories
{
    public class PipelineRepository : IPipelineRepository
    {
        private ILogger<PipelineRepository> _logger;
        private readonly RepositoryDbContext _repositoryDbContext;

        public PipelineRepository(ILogger<PipelineRepository> logger, RepositoryDbContext repositoryDbContext)
        {
            _logger = logger;
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task<Pipeline> AddPipeline(Pipeline pipeline)
        {
            await _repositoryDbContext.Pipelines.AddAsync(pipeline);
            _repositoryDbContext.SaveChanges();
            return pipeline;
        }

        public async Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId)
        {
            return await _repositoryDbContext.Pipelines.FirstOrDefaultAsync(p => p.Id == pipelineId && p.RepositoryId == repositoryId);
        }

        public async Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId)
        {
            return await _repositoryDbContext.Pipelines.Where(p => p.RepositoryId == repositoryId).ToListAsync();
        }

        public async Task<bool> DeletePipeline(Guid organisationId, Guid repositoryId, Guid pipelineId)
        {
            var pipeline = await _repositoryDbContext.Pipelines.FirstAsync(p => p.Id == pipelineId);

            if (pipeline == null)
            {
                _logger.LogInformation($"Pipeline with ID {pipelineId} not found.");
                return false;
            }

            _repositoryDbContext.Pipelines.Remove(pipeline);
            await _repositoryDbContext.SaveChangesAsync();

            return true;
        }
    }
}
