using DAPM.Orchestrator.Consumers.ResultConsumers;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.Orchestrator.Processes
{
    public class PostRegistrationRequestProcess : OrchestratorProcess
    {
        private string _username;
        private string _password;
        private string _name;
        private string _role;
        private Guid _ticketId;

        public PostRegistrationRequestProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, string UserName, string PassWord, string Name, string Role) : base(engine, serviceProvider, processId)
        {
            _username = UserName;
            _password = PassWord;
            _ticketId = ticketId;
            _name = Name;
            _role = Role;
        }

        public override void StartProcess()
        {
            var postRegistrationProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostRegistrationMessage>>();

            var message = new PostRegistrationMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Username = _username,
                Password = _password,
                Name = _name,
                Role = _role
            };

            postRegistrationProducer.PublishMessage(message);
        }

        public override void OnPostRegistrationResult(PostRegistrationResultMessage message)
        {

            var postRegistrationProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostRegistrationProcessResult>>();
            var processResultMessage = new PostRegistrationProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Succeeded = message.Succeeded,
            };

            postRegistrationProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
