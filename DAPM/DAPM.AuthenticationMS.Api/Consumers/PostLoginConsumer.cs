using DAPM.AuthenticationMS.Api.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.AuthenticationMS.Api.Consumers
{
    public class PostLoginConsumer : IQueueConsumer<PostLoginMessage>
    {
        private ILogger<PostLoginConsumer> _logger;
        private IAuthenticationService _authenticationService;
        private IQueueProducer<PostLoginResultMessage> _postLoginResultProducer;
        public PostLoginConsumer(ILogger<PostLoginConsumer> logger,
            IQueueProducer<PostLoginResultMessage> postLoginResultProducer,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _postLoginResultProducer = postLoginResultProducer;
            _authenticationService = authenticationService;
        }
        public async Task ConsumeAsync(PostLoginMessage message)
        {
            _logger.LogInformation("PostLoginMessage received");

            var username = message.Username;
            var password = message.Password;

            //TODO: Implement authentication logic
            

            var resultMessage = new PostLoginResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId,
                Succeeded = true
            };

            _postLoginResultProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
