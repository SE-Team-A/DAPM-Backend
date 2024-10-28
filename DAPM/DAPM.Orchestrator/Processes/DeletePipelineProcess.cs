using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.Repository;

namespace DAPM.Orchestrator.Processes
{
    public class DeletePipelineProcess : OrchestratorProcess
    {
        private readonly Guid _organizationId;
        private readonly Guid _repositoryId;
        private readonly Guid _ticketId;
        private readonly Guid _pipelineId;
        public DeletePipelineProcess(
            OrchestratorEngine engine,
            IServiceProvider serviceProvider,
            Guid processId,
            Guid organizationId,
            Guid repositoryId,
            Guid ticketId,
            Guid pipelineId) :
            base(engine, serviceProvider, processId)
        {
            _pipelineId = pipelineId;
            _ticketId = ticketId;
            _repositoryId = repositoryId;
            _organizationId = organizationId;
        }
        public override void StartProcess()
        {
            var deletePipelineProducer = _serviceScope.ServiceProvider
                .GetRequiredService<IQueueProducer<DeleteRepositoryPipelineMessage>>();

            var message = new DeleteRepositoryPipelineMessage
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                PipelineId = _pipelineId
            };

            deletePipelineProducer.PublishMessage(message);
        }

        public override void OnDeleteRepositoryPipelineResult(DeleteRepositoryPipelineResultMessage message)
        {

        }

        public override void OnDeleteRegistryPipelineResult(DeleteRegistryPipelineResultMessage message)
        {
            var deleteRegistryPipelineMessageProducer = _serviceScope.ServiceProvider
                            .GetRequiredService<IQueueProducer<DeleteRegistryPipelineResult>>();

            var processResultMessage = new DeleteRegistryPipelineResult
            {
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                PipelineId = _pipelineId,
                TicketId = _ticketId
            };

            deleteRegistryPipelineMessageProducer.PublishMessage(processResultMessage);

            EndProcess();
        }
    }
}