using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.AuthenticationMS.Api.Consumers
{
    public class PostUserRoleConsumer : IQueueConsumer<PostUserRoleMessage>
    {
        private ILogger<PostRegistrationConsumer> _logger;
        private IUserService _userService;
        private IRolesService _roleService;
        private IQueueProducer<PostUserRoleResultMessage> _postUserRoleResultProducer;
        public PostUserRoleConsumer(ILogger<PostRegistrationConsumer> logger,
            IQueueProducer<PostUserRoleResultMessage> postUserRoleResultProducer,
            IUserService userService, IRolesService roleService)
        {
            _logger = logger;
            _postUserRoleResultProducer = postUserRoleResultProducer;
            _userService = userService;
            _roleService = roleService;
        }
        public async Task ConsumeAsync(PostUserRoleMessage message)
        {
            _logger.LogInformation($"Set user role process started. Token: {message.RequestToken}, UserId: {message.UserId}, RoleName: {message.RoleName}");

            var err = await _roleService.SetUserRole(message.RequestToken, message.UserId, message.RoleName);

            var resultMessage = new PostUserRoleResultMessage
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = message.ProcessId,
                Succeeded = err == null,
                ErrMsg = err,
            };

            _postUserRoleResultProducer.PublishMessage(resultMessage);

            return;
        }
    }
}
