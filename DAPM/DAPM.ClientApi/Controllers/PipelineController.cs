using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services.Interfaces;
using Grpc.Net.Client.Balancer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Models;
using Swashbuckle.AspNetCore.Annotations;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("organizations/")]
    public class PipelineController : ControllerBase
    {
        private readonly ILogger<PipelineController> _logger;
        private readonly IPipelineService _pipelineService;
        private readonly ITicketService _ticketService;


        public PipelineController(ILogger<PipelineController> logger,
            IPipelineService pipelineService,
            IQueueProducer<CreateInstanceExecutionMessage> createInstanceProducer,
            ITicketService ticketService)
        {
            _logger = logger;
            _pipelineService = pipelineService;
            _ticketService = ticketService;
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}")]
        [SwaggerOperation(Description = "Gets a pipeline by id. This endpoint includes the " +
            "pipeline model in JSON. You need to have a collaboration agreement to retrieve this information.")]
        public async Task<ActionResult<Guid>> GetPipelineById(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid id = _pipelineService.GetPipelineById(organizationId, repositoryId, pipelineId);
            return Ok(new ApiResponse { RequestName = "GetPipelineById", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}/executions")]
        [SwaggerOperation(Description = "Creates a new execution instance for a pipeline previously saved in the system. The execution is created but not started")]
        public async Task<ActionResult<Guid>> CreatePipelineExecutionInstance(Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            Guid id = _pipelineService.CreatePipelineExecution(organizationId, repositoryId, pipelineId);
            return Ok(new ApiResponse { RequestName = "CreatePipelineExecutionInstance", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}/executions/{executionId}/commands/start")]
        [SwaggerOperation(Description = "Posts a start command to the defined pipeline execution. The start command will start the pipeline execution.")]
        public async Task<ActionResult<Guid>> PostStartCommand(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId)
        {
            Guid id = _pipelineService.PostStartCommand(organizationId, repositoryId, pipelineId, executionId);
            return Ok(new ApiResponse { RequestName = "PostStartCommand", TicketId = id });
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/pipelines/{pipelineId}/executions/{executionId}/status")]
        [SwaggerOperation(Description = "Gets the status of a running execution")]
        public async Task<ActionResult<Guid>> GetPipelineExecutionStatus(Guid organizationId, Guid repositoryId, Guid pipelineId, Guid executionId)
        {
            Guid id = _pipelineService.GetExecutionStatus(organizationId, repositoryId, pipelineId, executionId);
            return Ok(new ApiResponse { RequestName = "GetExecutionStatus", TicketId = id });
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/pipeline")]
        [SwaggerOperation(Description = "Get all Pipelines for a Repository of the Organization")]
        public async Task<ActionResult<IEnumerable<PipelineDTO>>> GetPipelines(Guid organizationId, Guid repositoryId)
        {
            Guid id = _pipelineService.GetPipelinesForRepository(organizationId, repositoryId);

            var tries = 1;
            while (tries < 4)
            {
                var resp = _ticketService.GetTicketResolution(id);

                switch ((int)resp["status"])
                {
                    case 0:
                    {
                        tries++;
                        Thread.Sleep(2000);
                        break;
                    }
                    case 1:
                        return Ok(JsonConvert.SerializeObject(resp));
                    case 2:
                    case 3:
                        return NotFound($"No pipelines found for repository {repositoryId}.");
                }
            }

            return NotFound("Ticket Resolution timed out");
        }


    }
}
