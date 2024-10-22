using DAPM.AuthenticationMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.AuthenticationMS.Api.Consumers
{
    public class PostRegistrationConsumer : IQueueConsumer<PostRegistrationMessage>
    {
        private ILogger<PostRegistrationConsumer> _logger;
        private IUserService _userService;
        private IQueueProducer<PostRegistrationResultMessage> _postRegistrationResultProducer;
        public PostRegistrationConsumer(ILogger<PostRegistrationConsumer> logger,
            IQueueProducer<PostRegistrationResultMessage> postRegistrationResultProducer,
            IUserService userService)
        {
            _logger = logger;
            _postRegistrationResultProducer = postRegistrationResultProducer;
            _userService = userService;
        }
        public async Task ConsumeAsync(PostRegistrationMessage message)
        {
            _logger.LogInformation("PostRegistrationMessage received");

            var username = message.Username;
            var password = message.Password;
            // drunk, fix later
            // var name = message.Name;
            var role = message.Role == "admin";

            var result = await _userService.CreateUserAsync(username, password, role);

            var resultMessage = new PostRegistrationResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId,
                Succeeded = result.Succeeded
            };

            _postRegistrationResultProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
