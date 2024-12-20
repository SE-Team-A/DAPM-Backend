using System.Security.Principal;
using DAPM.Orchestrator.Consumers.ResultConsumers;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

/// <author>Vladyslav Synytskyi</author>

namespace DAPM.Orchestrator.Processes
{
    public class DeleteUserProcess : OrchestratorProcess
    {
        private string _token;
        private Guid _userId;
        private Guid _ticketId;

        public DeleteUserProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, string token, Guid userId) : base(engine, serviceProvider, processId)
        {
            _ticketId = ticketId;
            _token = token;
            _userId = userId;
        }

        public override void StartProcess()
        {
            var deleteUserMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<DeleteUserMessage>>();

            var message = new DeleteUserMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                RequestToken = _token,
                UserId = _userId,
            };

            deleteUserMessageProducer.PublishMessage(message);
        }

        public override void OnDeleteUserResult(DeleteUserResultMessage message)
        {

            var deleteUserProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<DeleteUserProcessResult>>();
            var processResultMessage = new DeleteUserProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = message.Succeeded,
                ErrMsg = message.ErrMsg,
            };

            deleteUserProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}