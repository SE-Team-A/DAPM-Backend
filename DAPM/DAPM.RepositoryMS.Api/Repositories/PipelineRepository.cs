using DAPM.RepositoryMS.Api.Data;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
/// <author>Raihanullah Mehran</author>
/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
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

        public async Task<Pipeline> EditPipeline(Pipeline pipeline, Guid pipelineId)
        {

            var found = await _repositoryDbContext.Pipelines.FirstOrDefaultAsync(p => p.Id == pipelineId && p.RepositoryId == pipeline.RepositoryId);

            found.PipelineJson = pipeline.PipelineJson;

            // _repositoryDbContext.Entry(found).CurrentValues.SetValues(pipeline.PipelineJson);

            _repositoryDbContext.SaveChanges();
            return pipeline;
        }

        public async Task<PipelineExecution> AddPipelineExecution(PipelineExecution pipelineExecution)
        {
            await _repositoryDbContext.PipelineExecutions.AddAsync(pipelineExecution);
            _repositoryDbContext.SaveChanges();
            return pipelineExecution;
        }

        public async Task<Pipeline> GetPipelineById(Guid repositoryId, Guid pipelineId)
        {
            return await _repositoryDbContext.Pipelines.FirstOrDefaultAsync(p => p.Id == pipelineId && p.RepositoryId == repositoryId);
        }

        public async Task<IEnumerable<Pipeline>> GetPipelines(Guid repositoryId)
        {
            return await _repositoryDbContext.Pipelines.Where(p => p.RepositoryId == repositoryId).ToListAsync();
        }

        public async Task<IEnumerable<PipelineExecution>> GetPipelineExecutions(Guid pipelineId)
        {
            return await _repositoryDbContext.PipelineExecutions.Where(p => p.PipelineId == pipelineId).ToListAsync();
        }

        public async Task<Tuple<Pipeline, PipelineExecution>> GetPipelineExecutionById(Guid executionId)
        {
            PipelineExecution ex = await _repositoryDbContext.PipelineExecutions.Where(p => p.Id == executionId).FirstOrDefaultAsync();
            Pipeline pip = await _repositoryDbContext.Pipelines.FirstOrDefaultAsync(p => p.Id == ex.PipelineId);

            return new Tuple<Pipeline, PipelineExecution>(pip, ex);
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
