using DAPM.Orchestrator.Consumers.ResultConsumers;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class PostLoginRequestProcess : OrchestratorProcess
    {
        private string _username;
        private string _password;
        private Guid _ticketId;

        public PostLoginRequestProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, string UserName, string PassWord) : base(engine, serviceProvider, processId)
        {
            _username = UserName;
            _password = PassWord;
            _ticketId = ticketId;
        }

        public override void StartProcess()
        {
            var postLoginProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostLoginMessage>>();

            var message = new PostLoginMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Username = _username,
                Password = _password
            };

            postLoginProducer.PublishMessage(message);
        }

        public override void OnPostLoginResult(PostLoginResultMessage message)
        {

            var postLoginProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostLoginProcessResult>>();
            var processResultMessage = new PostLoginProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = message.Succeeded,
                Token = message.Token
            };

            postLoginProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
