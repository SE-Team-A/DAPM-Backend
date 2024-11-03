using DAPM.Orchestrator.Consumers.ResultConsumers;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Authentication;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.ResourceRegistry;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.Orchestrator.Processes
{
    public class GetAllUsersRequestProcess : OrchestratorProcess
    {
        private Guid _ticketId;
        private string _token;

        public GetAllUsersRequestProcess(OrchestratorEngine engine, IServiceProvider serviceProvider,
            Guid ticketId, Guid processId, string Token) : base(engine, serviceProvider, processId)
        {
            _token = Token;
            _ticketId = ticketId;
        }

        public override void StartProcess()
        {
            var getAllUsersProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetAllUsersMessage>>();

            var message = new GetAllUsersMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Token = _token
            };

            getAllUsersProducer.PublishMessage(message);
        }

        public override void OnGetAllUsersResult(GetAllUsersResultMessage message)
        {

            var getAllUsersProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetAllUsersProcessResult>>();
            var processResultMessage = new GetAllUsersProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Token = message.Token
            };

            getAllUsersProcessResultProducer.PublishMessage(processResultMessage);

            EndProcess();

        }
    }
}
