﻿using DAPM.RepositoryMS.Api.Data;
using DAPM.RepositoryMS.Api.Models.PostgreSQL;
using DAPM.RepositoryMS.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
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

        public Task<PipelineExecution> AddPipelineExecution(RabbitMQLibrary.Models.PipelineExecution pipelineExecution)
        {
            throw new NotImplementedException();
        }
    }
}
