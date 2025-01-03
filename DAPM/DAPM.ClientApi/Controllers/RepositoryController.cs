﻿using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Models;
using Swashbuckle.AspNetCore.Annotations;

/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
/// <author>Tamas Drabos</author>
namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("organizations/")]
    public class RepositoryController : ControllerBase
    {

        private readonly ILogger<RepositoryController> _logger;
        private readonly IRepositoryService _repositoryService;
        
        public RepositoryController(ILogger<RepositoryController> logger, IRepositoryService repositoryService)
        {
            _logger = logger;
            _repositoryService = repositoryService;
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}")]
        [SwaggerOperation(Description = "Gets a repository by id. You need to have a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> GetRepositoryById(Guid organizationId, Guid repositoryId)
        {
            Guid id = _repositoryService.GetRepositoryById(organizationId, repositoryId);
            return Ok(new ApiResponse { RequestName = "GetRepositoryById", TicketId = id });
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/resources")]
        [SwaggerOperation(Description = "Gets the resources in a repository by id. The result of this endpoint " +
            "does not include the resource files. You need to have a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> GetResourcesOfRepository(Guid organizationId, Guid repositoryId)
        {
            Guid id = _repositoryService.GetResourcesOfRepository(organizationId, repositoryId);
            return Ok(new ApiResponse { RequestName = "GetResourcesOfRepository", TicketId = id });
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/pipelines")]
        [SwaggerOperation(Description = "Gets the pipelines of a repository by id. The result of this endpoint " +
            "does not include the JSON models of the pipelines. You need to have a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> GetPipelinesOfRepository(Guid organizationId, Guid repositoryId)
        {
            Guid id = _repositoryService.GetPipelinesOfRepository(organizationId, repositoryId);
            return Ok(new ApiResponse { RequestName = "GetPipelinesOfRepository", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/resources")]
        [SwaggerOperation(Description = "Posts a new resource into a repository by id.")]
        public async Task<ActionResult<Guid>> PostResourceToRepository(Guid organizationId, Guid repositoryId, [FromForm] ResourceForm resourceForm)
        {
            if (resourceForm.Name == null || resourceForm.ResourceFile == null)
                return BadRequest();

            Guid id = _repositoryService.PostResourceToRepository(organizationId, repositoryId, resourceForm.Name, resourceForm.ResourceFile, resourceForm.ResourceType);
            return Ok(new ApiResponse { RequestName = "PostResourceToRepository", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/resources/operators")]
        [SwaggerOperation(Description = "Posts a new operator resource into a repository by id. In this endpoint you have to provide the source code for the operator and a " +
            "Dockerfile to build it and execute it.")]
        public async Task<ActionResult<Guid>> PostOperatorToRepository(Guid organizationId, Guid repositoryId, [FromForm] OperatorForm resourceForm)
        {
            if (resourceForm.Name == null || resourceForm.SourceCodeFile == null)
                return BadRequest();

            Guid id = _repositoryService.PostOperatorToRepository(organizationId, repositoryId, resourceForm.Name,
                resourceForm.SourceCodeFile, resourceForm.DockerfileFile, resourceForm.ResourceType);
            return Ok(new ApiResponse { RequestName = "PostOperatorToRepository", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines")]
        [SwaggerOperation(Description = "Posts a new pipeline into a repository by id. In this endpoint you have to provide the JSON model of the pipeline based on the model" +
            " we agreed on.")]
        public async Task<ActionResult<Guid>> PostPipelineToRepository(Guid organizationId, Guid repositoryId, [FromBody] PipelineApiDto pipelineApiDto)
        {
            Guid id = _repositoryService.PostPipelineToRepository(organizationId, repositoryId, pipelineApiDto);
            return Ok(new ApiResponse { RequestName = "PostPipelineToRepository", TicketId = id });
        }


        [HttpPut("{organizationId}/repositories/{repositoryId}/pipeline/{pipelineId}")]
        [SwaggerOperation(Description = "Edits a pipeline by id. This endpoint includes the " +
            "pipeline model in JSON. You need to have a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> EditPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId, [FromBody]PipelineApiDto pipelineApiDto)
        {
            Guid id = _repositoryService.EditPipelineById(organizationId, repositoryId, pipelineId, pipelineApiDto);
            return Ok(new ApiResponse { RequestName = "EditPipelineById", TicketId = id });
        }

        /// <author>Ayat Al Rifai</author>
        [HttpDelete("{organizationId}/repositories/{repositoryId}/resources/{resourceId}")]
        [SwaggerOperation(Description = "Marks a resource as deleted in a specific repository.")]
        public async Task<IActionResult> DeleteResourceById(Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            Guid id = _repositoryService.DeleteResourceById(organizationId, repositoryId, resourceId);
            if (id != Guid.Empty)
            {
                return Ok(new { success = true, message = id });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to delete resource." });
            }

        }

        [HttpDelete("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}")]
        [SwaggerOperation(Description = "This endpoint deleteds a pipeline")]
        public async Task<IActionResult> DeletePipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid id = _repositoryService.DeletePipelineById(organizationId, repositoryId, pipelineId);

            if (id != Guid.Empty)
            {
                return Ok(new { success = true, message = id });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to delete pipeline." });
            }
        }

    }
}
