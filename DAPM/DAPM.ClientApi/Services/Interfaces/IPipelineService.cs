﻿using DAPM.ClientApi.Models.DTOs;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IPipelineService
    {
        public Guid GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId);
        public Guid GetPipelinesForRepository(Guid organizationId, Guid repositoryId);
        public Guid CreatePipelineExecution(Guid organizationId, Guid repositoryId, PipelineExecutionApiDto pipelineId);
        public Guid PostStartCommand(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId);
        public Guid GetExecutionStatus(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId);
    }
}
