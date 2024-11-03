using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.Authentication;
using Microsoft.Extensions.Logging;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ClientApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IQueueProducer<PostLoginRequest> _postloginRequestProducer;
        private readonly IQueueProducer<PostRegistrationRequest> _postregistrationRequestProducer;
        private readonly ITicketService _ticketService;
        private readonly IQueueProducer<GetAllUsersRequest> _getAllUsersRequestProducer;

        public AuthenticationService(ILogger<AuthenticationService> logger,
            IQueueProducer<PostLoginRequest> postloginRequestProducer,
            IQueueProducer<PostRegistrationRequest> postregistrationRequestProducer,
            ITicketService ticketService,
            IQueueProducer<GetAllUsersRequest> getAllUsersRequestProducer)
        {
            _logger = logger;
            _ticketService = ticketService;
            _postloginRequestProducer = postloginRequestProducer;
            _postregistrationRequestProducer = postregistrationRequestProducer;
            _getAllUsersRequestProducer = getAllUsersRequestProducer;
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

        public Guid PostRegistration(string username, string password, string name, string role)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);
            
            var message = new PostRegistrationRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Username = username,
                Password = password,
                Name = name,
                Role = role
            };

            _postregistrationRequestProducer.PublishMessage(message);

            _logger.LogDebug("Registration Request Enqueued");

            return ticketId;
        }

        public Guid GetAllUsers(string token)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);
            
            var message = new GetAllUsersRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Token = token
            };

            _getAllUsersRequestProducer.PublishMessage(message);

            _logger.LogDebug("GetAllUsers Request Enqueued");

            return ticketId;
        }
    }
}
