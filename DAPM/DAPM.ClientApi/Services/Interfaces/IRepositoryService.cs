﻿using DAPM.ClientApi.Models.DTOs;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IRepositoryService
    {
        public Guid GetRepositoryById(Guid organizationId, Guid repositoryId);
        public Guid GetResourcesOfRepository(Guid organizationId, Guid repositoryId);
        public Guid GetPipelinesOfRepository(Guid organizationId, Guid repositoryId);
        public Guid PostResourceToRepository(Guid organizationId, Guid repositoryId, string name, IFormFile resourceFile, string resourceType);
        public Guid PostOperatorToRepository(Guid organizationId, Guid repositoryId, string name, IFormFile sourceCodeFile, IFormFile dockerfileFile, string resourceType);
        public Guid PostPipelineToRepository(Guid organizationId, Guid repositoryId, PipelineApiDto pipeline);
        public Guid DeleteResourceById(Guid organizationId, Guid repositoryId, Guid resourceId);
       public Guid EditPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId, PipelineApiDto pipelineApiDto);

        public Guid DeletePipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId);
        
    }
}
