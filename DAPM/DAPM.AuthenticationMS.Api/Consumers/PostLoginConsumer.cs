using DAPM.AuthenticationMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.AuthenticationMS.Api.Consumers
{
    public class PostLoginConsumer : IQueueConsumer<PostLoginMessage>
    {
        private ILogger<PostLoginConsumer> _logger;
        private IUserService _userService;
        private IQueueProducer<PostLoginResultMessage> _postLoginResultProducer;
        public PostLoginConsumer(ILogger<PostLoginConsumer> logger,
            IQueueProducer<PostLoginResultMessage> postLoginResultProducer,
            IUserService userService)
        {
            _logger = logger;
            _postLoginResultProducer = postLoginResultProducer;
            _userService = userService;
        }
        public async Task ConsumeAsync(PostLoginMessage message)
        {
            _logger.LogInformation("PostLoginMessage received");

            var username = message.Username;
            var password = message.Password;

            var resultMessage = new PostLoginResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId,
                Token = await _userService.LoginUserAsync(username, password),
            };

            if (resultMessage.Token != null) {
                resultMessage.Succeeded = true;
            }

            _postLoginResultProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
