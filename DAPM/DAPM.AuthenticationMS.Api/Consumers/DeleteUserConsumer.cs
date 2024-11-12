using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.AuthenticationMS.Api.Consumers
{
    public class DeleteUserConsumer : IQueueConsumer<DeleteUserMessage>
    {
        private ILogger<DeleteUserConsumer> _logger;
        private IUserService _userService;
        private IRolesService _roleService;

        private IQueueProducer<DeleteUserResultMessage> _deleteUserResultProducer;
        public DeleteUserConsumer(ILogger<DeleteUserConsumer> logger,
            IQueueProducer<DeleteUserResultMessage> deleteUserResultProducer,
            IUserService userService, IRolesService roleService)
        {
            _logger = logger;
            _deleteUserResultProducer = deleteUserResultProducer;
            _userService = userService;
            _roleService = roleService;
        }
        public async Task ConsumeAsync(DeleteUserMessage message)
        {
            _logger.LogInformation($"Delete user process started. Token: {message.RequestToken}, UserId: {message.UserId}");

            await _userService.DeleteUserFromSystem(message.RequestToken, message.UserId);

            var resultMessage = new DeleteUserResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId
            };

            _deleteUserResultProducer.PublishMessage(resultMessage);

            return;
        }
    }
}