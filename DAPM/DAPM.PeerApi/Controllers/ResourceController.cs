using DAPM.PeerApi.Models;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("resources/")]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;
        private IQueueProducer<PostResourceFromPeerRequest> _postResourceQueueProducer;
        private IQueueProducer<SendResourceToPeerResultMessage> _sendResourceToPeerResultQueueProducer;
        private readonly IHttpService _httpService;

        public ResourceController(ILogger<ResourceController> logger,
            IQueueProducer<PostResourceFromPeerRequest> postResourceQueueProducer,
            IQueueProducer<SendResourceToPeerResultMessage> sendResourceToPeerResultQueueProducer,
            IHttpService httpService)
        {
            _logger = logger;
            _postResourceQueueProducer = postResourceQueueProducer;
            _sendResourceToPeerResultQueueProducer = sendResourceToPeerResultQueueProducer;
            _httpService = httpService;
        }

        [HttpPost]
        public async Task<ActionResult> PostResource([FromBody] SendResourceToPeerDto sendResourceToPeerDto)
        {
            if (!await _httpService.verifyExternalToken(sendResourceToPeerDto.SenderPeerIdentity.Domain, Request.Headers["Authorization"].FirstOrDefault())) {
                return Unauthorized();
            }
            _logger.LogInformation("Ticket id / step id in post resource endpoint is " + sendResourceToPeerDto.StepId.ToString());
            var message = new PostResourceFromPeerRequest()
            {
                ExecutionId = sendResourceToPeerDto.ExecutionId,
                SenderProcessId = sendResourceToPeerDto.StepId,
                RepositoryId = sendResourceToPeerDto.RepositoryId,
                Resource = sendResourceToPeerDto.Resource,
                SenderPeerIdentity = sendResourceToPeerDto.SenderPeerIdentity,
                StorageMode = sendResourceToPeerDto.StorageMode,
                TimeToLive = TimeSpan.FromMinutes(1),
            };

            _postResourceQueueProducer.PublishMessage(message);
            return Ok("Post resource received");
        }

        [HttpPost("result")]
        public async Task<ActionResult> PostResourceResult([FromBody] SendResourceToPeerResultDto sendResourceToPeerResultDto)
        {
            _logger.LogInformation("Ticket id / step id in post resource result endpoint is " + sendResourceToPeerResultDto.StepId.ToString());
            var message = new SendResourceToPeerResultMessage()
            {
                SenderProcessId = sendResourceToPeerResultDto.StepId,
                Succeeded = sendResourceToPeerResultDto.Succeeded,
                TimeToLive = TimeSpan.FromMinutes(1),
            };

            _sendResourceToPeerResultQueueProducer.PublishMessage(message);
            return Ok("Post resource result received");
        }
    }
}
