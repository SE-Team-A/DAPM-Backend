using System.Security.Principal;
using DAPM.Orchestrator.Consumers.ResultConsumers;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

/// <author>Ákos Gelencsér</author>
namespace DAPM.Orchestrator.Processes
{
    public class PostUserRoleProcess : OrchestratorProcess
    {
        private string _token;
        private Guid _userId;
        private string _roleName;
        private Guid _ticketId;

        public PostUserRoleProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, string token, Guid userId, string roleName) : base(engine, serviceProvider, processId)
        {
            _ticketId = ticketId;
            _token = token;
            _userId = userId;
            _roleName = roleName;
        }

        public override void StartProcess()
        {
            var postUserRoleMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostUserRoleMessage>>();

            var message = new PostUserRoleMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RequestToken = _token,
                UserId = _userId,
                RoleName = _roleName,
            };

            postUserRoleMessageProducer.PublishMessage(message);
        }

        public override void OnPostUserRoleResult(PostUserRoleResultMessage message)
        {

            var postUserRoleProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostUserRoleProcessResult>>();
            var processResultMessage = new PostUserRoleProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = message.Succeeded,
                ErrMsg = message.ErrMsg,
            };

            postUserRoleProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
