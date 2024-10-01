using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DAPM.ClientApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IQueueProducer<PostLoginRequest> _loginRequestProducer;
        private readonly IQueueConsumer<LoginResponse> _loginResponseConsumer;
        private readonly ITicketService _ticketService;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            IQueueProducer<PostLoginRequest> loginRequestProducer,
            IQueueConsumer<LoginResponse> loginResponseConsumer,
            ITicketService ticketService)
        {
            _logger = logger;
            _loginRequestProducer = loginRequestProducer;
            _loginResponseConsumer = loginResponseConsumer;
            _ticketService = ticketService;
        }

        public async Task<string> RequestJwtTokenAsync(string username, string password)
        {
            // Create a new ticket for this authentication request
            var ticketId = _ticketService.CreateNewTicket(TicketResolutionType.Json);
            
            // Create the login request message
            var loginRequest = new PostLoginRequest
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                TicketId = ticketId,
                Username = username,
                Password = password
            };

            try
            {
                _loginRequestProducer.PublishMessage(loginRequest);
                _logger.LogInformation("Login request published for username: {Username}", username);

                // Wait for the login response using the ticket ID
                var loginResponse = await _loginResponseConsumer.ConsumeMessageAsync(ticketId.ToString());

                if (loginResponse != null && loginResponse.Success)
                {
                    _logger.LogInformation("JWT token received for user: {Username}", username);
                    return loginResponse.Token;
                }

                _logger.LogWarning("Login failed for user: {Username}", username);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while requesting JWT token for user: {Username}", username);
                return null;
            }
        }
    }
}
