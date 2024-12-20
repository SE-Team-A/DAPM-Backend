using DAPM.Orchestrator.Consumers.StartProcessConsumers;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;

/// <author>Tamas Drabos</author>

namespace DAPM.Orchestrator.Processes
{
    public class GetPipelineExecutionsProcess: OrchestratorProcess
    {
        private Guid _repositoryId;
        private Guid _pipelineId;
        private Guid _processId;
        private Guid _ticketId;

        public GetPipelineExecutionsProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid processId, Guid repositoryId, Guid pipelineId) : base(engine, serviceProvider, processId)
        {
            _repositoryId = repositoryId;
            _pipelineId = pipelineId;
            _processId = processId;
            _ticketId = ticketId;
        }

        public override void StartProcess()
        {
            var getPipelineExecutionsFromRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelineExecutionsFromRepoMessage>>();

            var msg = new GetPipelineExecutionsFromRepoMessage()
            {
                PipelineId = _pipelineId,
                RepositoryId = _repositoryId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ProcessId = _processId
            };

            getPipelineExecutionsFromRepoProducer.PublishMessage(msg);
        }

        public override void OnGetPipelineExecutionsFromRepoResult(GetPipelineExecutionsFromRepoResultMessage message)
        {
            var getPipelineExecutionsProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetPipelineExecutionsProcessResult>>();

            var resultMessage = new GetPipelineExecutionsProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PipelineExecutions = message.PipelineExecutions
            };

            getPipelineExecutionsProcessResultProducer.PublishMessage(resultMessage);

        }
    }
}
