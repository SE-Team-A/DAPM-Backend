using DAPM.AuthenticationMS.Api.Services;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.AuthenticationMS.Api.Consumers
{
    public class GetAllUsersConsumer : IQueueConsumer<GetAllUsersMessage>
    {
        private ILogger<GetAllUsersConsumer> _logger;
        private IUserService _userService;
        private IQueueProducer<GetAllUsersResultMessage> _getAllUsersResultProducer;
        public GetAllUsersConsumer(ILogger<GetAllUsersConsumer> logger,
            IQueueProducer<GetAllUsersResultMessage> getAllUsersResultProducer,
            IUserService userService)
        {
            _logger = logger;
            _getAllUsersResultProducer = getAllUsersResultProducer;
            _userService = userService;
        }
        public async Task ConsumeAsync(GetAllUsersMessage message)
        {
            _logger.LogInformation("GetAllUsersMessage received");

            List<UserDto> users = await _userService.GetAllUsersAsync(message.Token);

            var resultMessage = new GetAllUsersResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId,
                UserDtos = users
            };

            _getAllUsersResultProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
