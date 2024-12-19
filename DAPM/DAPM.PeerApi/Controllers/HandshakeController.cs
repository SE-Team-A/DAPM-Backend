using DAPM.PeerApi.Models.HandshakeDtos;
using DAPM.PeerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.PeerApi.Controllers
{
    [ApiController]
    [Route("handshake/")]
    public class HandshakeController : ControllerBase
    {
        private readonly ILogger<HandshakeController> _logger;
        private IHandshakeService _handshakeService;
        private readonly IHttpService _httpService;

        public HandshakeController(ILogger<HandshakeController> logger, IHandshakeService handshakeService, IHttpService httpService)
        {
            _logger = logger;
            _handshakeService = handshakeService;
            _httpService = httpService;
        }

        [HttpPost("request")]
        public async Task<ActionResult> PostHandshakeRequest([FromBody] HandshakeRequestDto requestDto)
        {
            if (!await _httpService.verifyExternalToken(requestDto.SenderIdentity.Domain, Request.Headers["Authorization"].FirstOrDefault())) {
                return Unauthorized();
            }

            _handshakeService.OnHandshakeRequest(requestDto.HandshakeId, requestDto.SenderIdentity);
            return Ok("Handshake request received");
        }

        [HttpPost("request-response")]
        public async Task<ActionResult> PostHandshakeRequestResponse([FromBody] HandshakeRequestResponseDto requestResponseDto)
        {
            if (!await _httpService.verifyExternalToken(requestResponseDto.SenderIdentity.Domain, Request.Headers["Authorization"].FirstOrDefault())) {
                return Unauthorized();
            }
            _handshakeService.OnHandshakeRequestResponse(requestResponseDto.HandshakeId, 
                requestResponseDto.SenderIdentity, requestResponseDto.IsAccepted);
            return Ok("Handshake request response received");
        }


    }
}
