using DAPM.PeerApi.Models;
using DAPM.PeerApi.Services;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("token/")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;
        private readonly ITokenService _tokenService;

        public TokenController(ILogger<ResourceController> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost("verify")]
        public async Task<ActionResult> VerifyToken([FromBody] VerifyTokenDto verifyTokenDto)
        {
            _logger.LogInformation($"Token sent for verification {verifyTokenDto.Token}");

            if (_tokenService.checkSignature(verifyTokenDto.Token)) {
                return Ok();
            }

            return Unauthorized();
        }
    }
}
