﻿using DAPM.ResourceRegistryMS.Api.Models;

namespace DAPM.ResourceRegistryMS.Api.Services.Interfaces
{
    public interface IPipelineService
    {
        Task<Pipeline> GetPipelineById(Guid organizationId, Guid repositoryId, Guid resourceId);
        Task<bool> DeletePipeline(Guid organizationId, Guid repositoryId, Guid pipelineId);
    }
}
