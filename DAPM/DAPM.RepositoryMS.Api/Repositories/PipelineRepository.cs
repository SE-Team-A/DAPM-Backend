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

        public PipelineRepository(ILogger<PipelineRepository> logger,  RepositoryDbContext repositoryDbContext)
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
        public async Task<Pipeline> EditPipeline(Pipeline pipeline, Guid pipelineId)
        {
            
            var found = await _repositoryDbContext.Pipelines.FirstOrDefaultAsync(p => p.Id == pipelineId && p.RepositoryId == pipeline.RepositoryId);
        
            found.PipelineJson=pipeline.PipelineJson;    

           // _repositoryDbContext.Entry(found).CurrentValues.SetValues(pipeline.PipelineJson);

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
    }
}
