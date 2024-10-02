using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using Microsoft.Extensions.Logging;

namespace DAPM.ClientApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IQueueProducer<PostLoginRequest> _postloginRequestProducer;
        private readonly ITicketService _ticketService;

        public AuthenticationService(ILogger<AuthenticationService> logger,
            IQueueProducer<PostLoginRequest> postloginRequestProducer,
            ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
            _postloginRequestProducer = postloginRequestProducer;
        }

        public Guid PostLogin(string username, string password)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);
            
            var message = new PostLoginRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Username = username,
                Password = password
            };

            _postloginRequestProducer.PublishMessage(message);

            _logger.LogDebug("LoginRequest Enqueued");

            return ticketId;
        }
    }
}
