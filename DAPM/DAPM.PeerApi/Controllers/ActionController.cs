using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using DAPM.PipelineOrchestratorMS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("actions/")]
    public class ActionController : ControllerBase
    {
        private readonly ILogger<ActionController> _logger;
        private readonly IActionService _actionService;
        private readonly IHttpService _httpService;

        public ActionController(ILogger<ActionController> logger, IActionService actionService, IHttpService httpService)
        {
            _logger = logger;
            _actionService = actionService;
            _httpService = httpService;
        }

        [HttpPost("transfer-data")]
        public async Task<ActionResult> PostSendDataAction([FromBody] TransferDataActionDto actionDto)
        {
            if (!await _httpService.verifyExternalToken(actionDto.SenderIdentity.Domain, Request.Headers["Authorization"].FirstOrDefault())) {
                return Unauthorized();
            }
            _actionService.OnTransferDataActionReceived(actionDto.SenderProcessId, actionDto.SenderIdentity, actionDto.StepId, actionDto.Data);
            return Ok();
        }

        [HttpPost("execute-operator")]
        public async Task<ActionResult> PostExecuteOperatorAction([FromBody] ExecuteOperatorActionDto actionDto)
        {
            if (!await _httpService.verifyExternalToken(actionDto.SenderIdentity.Domain, Request.Headers["Authorization"].FirstOrDefault())) {
                return Unauthorized();
            }
            _actionService.OnExecuteOperatorActionReceived(actionDto.SenderProcessId, actionDto.SenderIdentity, actionDto.StepId, actionDto.Data);
            return Ok();
        }

        [HttpPost("action-result")]
        public async Task<ActionResult> PostActionResult([FromBody] ActionResultDto actionResultDto)
        {
            var actionResultDTO = new ActionResultDTO()
            {
                ActionResult = (PipelineOrchestratorMS.Api.Models.ActionResult)actionResultDto.ActionResult,
                ExecutionId = actionResultDto.ExecutionId,
                StepId = actionResultDto.StepId,
                Message = actionResultDto.Message,
            };

            _actionService.OnActionResultReceived(actionResultDto.ProcessId, actionResultDTO);
            return Ok();
        }

    }
}
