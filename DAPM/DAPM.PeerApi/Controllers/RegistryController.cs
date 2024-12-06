using DAPM.PeerApi.Models;
using DAPM.PeerApi.Models.ActionsDtos;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("registry/")]
    public class RegistryController : ControllerBase
    {
        private readonly ILogger<RegistryController> _logger;
        private IRegistryService _registryService;
        private IQueueProducer<RegistryUpdateMessage> _registryUpdateProducer;
        private readonly IHttpService _httpService;

        public RegistryController(ILogger<RegistryController> logger, 
            IQueueProducer<RegistryUpdateMessage> registryUpdateProducer,
            IRegistryService registryService,
            IHttpService httpService)
        {
            _logger = logger;
            _registryService = registryService;
            _registryUpdateProducer = registryUpdateProducer;
            _httpService = httpService;
        }

        [HttpPost("updates")]
        public async Task<ActionResult> PostRegistryUpdate([FromBody] RegistryUpdateDto registryUpdateDto)
        {
            if (!await _httpService.verifyExternalToken(registryUpdateDto.SenderIdentity.Domain, Request.Headers["Authorization"].FirstOrDefault())) {
                return Unauthorized();
            }
            _registryService.OnRegistryUpdate(registryUpdateDto);
            return Ok("Registry update received");
        }

        [HttpPost("update-ack")]
        public async Task<ActionResult> PostRegistryUpdateAck([FromBody] RegistryUpdateAckDto registryUpdateAckDto)
        {
            if (!await _httpService.verifyExternalToken(registryUpdateAckDto.SenderIdentity.Domain, Request.Headers["Authorization"].FirstOrDefault())) {
                return Unauthorized();
            }
            _registryService.OnRegistryUpdateAck(registryUpdateAckDto);
            return Ok("RegistryUpdate ack received");
        }
    }
}
