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
        private readonly IQueueProducer<PostUserRoleRequest> _postUserRoleRequestProducer;
        private readonly ITicketService _ticketService;
        private readonly IQueueProducer<GetAllUsersRequest> _getAllUsersRequestProducer;
        private readonly IQueueProducer<DeleteUserRequest> _deleteUserRequestProducer;

        public AuthenticationService(ILogger<AuthenticationService> logger,
            IQueueProducer<PostLoginRequest> postloginRequestProducer,
            IQueueProducer<PostRegistrationRequest> postregistrationRequestProducer,
            ITicketService ticketService,
            IQueueProducer<GetAllUsersRequest> getAllUsersRequestProducer,
            IQueueProducer<PostUserRoleRequest> postUserRoleRequestProducer,
            IQueueProducer<DeleteUserRequest> deleteUserRequestProducer)
        {
            _logger = logger;
            _ticketService = ticketService;
            _postloginRequestProducer = postloginRequestProducer;
            _postregistrationRequestProducer = postregistrationRequestProducer;
            _getAllUsersRequestProducer = getAllUsersRequestProducer;
            _postUserRoleRequestProducer = postUserRoleRequestProducer;
            _deleteUserRequestProducer = deleteUserRequestProducer;
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

        public Guid SetUserRole(string token, Guid userId, string roleName)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);
            
            var message = new PostUserRoleRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                RequestToken = token,
                UserId = userId,
                RoleName = roleName,
            };

            _postUserRoleRequestProducer.PublishMessage(message);

            _logger.LogDebug($"Set user role request enqueued for id {userId}, role {roleName}");

            return ticketId;
        }

        public Guid DeleteUserFromSystem(string token, Guid userId)
        {
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);
            
            var message = new DeleteUserRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                RequestToken = token,
                UserId = userId,
            };

            _deleteUserRequestProducer.PublishMessage(message);

            _logger.LogDebug($"Delete user request enqueued for id {userId}");

            return ticketId;
        }
    }
}
