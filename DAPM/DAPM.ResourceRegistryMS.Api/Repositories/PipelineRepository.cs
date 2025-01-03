﻿using DAPM.ResourceRegistryMS.Api.Models;
using DAPM.ResourceRegistryMS.Api.Repositories.Interfaces;
/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ResourceRegistryMS.Api.Repositories
{
    public class PipelineRepository : IPipelineRepository
    {
        private readonly ILogger<IPeerRepository> _logger;
        private readonly ResourceRegistryDbContext _context;

        public PipelineRepository(ILogger<IPeerRepository> logger, ResourceRegistryDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Pipeline> AddPipeline(Pipeline pipeline)
        {
            await _context.Pipelines.AddAsync(pipeline);
            _context.SaveChanges();
            return pipeline;
        }
        public async Task<Pipeline> EditPipeline(Pipeline pipeline, Guid pipelineId)
        {
            _context.SaveChanges();
            return pipeline;
        }

        public async Task<bool> DeletePipeline(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            var pipeline = _context.Pipelines.FirstOrDefault(p => p.Id == pipelineId);

            if (pipeline == null)
            {
                _logger.LogWarning($"Pipeline with ID: {pipelineId} and RepositoryID: {repositoryId} not found");
                return false;
            }

            _context.Pipelines.Remove(pipeline);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Pipeline> GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            return (Pipeline)_context.Resources.Where(r => r.PeerId == organizationId && r.RepositoryId == repositoryId && r.Id == pipelineId);
        }

        public async Task<IEnumerable<Pipeline>> GetPipelinesFromRepository(Guid organizationId, Guid repositoryId)
        {
            return _context.Pipelines.Where(r => r.PeerId == organizationId && r.RepositoryId == repositoryId);
        }
    }
}
