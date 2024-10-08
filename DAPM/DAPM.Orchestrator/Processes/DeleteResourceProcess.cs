using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using System.Runtime.CompilerServices;

namespace DAPM.Orchestrator.Processes
{
    public class DeleteResourceProcess: OrchestratorProcess
    {
        private Guid _organizationId;
        private Guid _repositoryId;
        private Guid _ticketId;
        private Guid _resourceId;

        public DeleteResourceProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid processId, 
            Guid organizationId, Guid repositoryId, Guid ticketId, Guid resourceId) : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _ticketId = ticketId;
            _resourceId = resourceId;
        }

        public override void StartProcess()
        {
            var deleteResourcesProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<DeleteResourceFromRepoMessage>>();

            var message = new DeleteResourceFromRepoMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                ResourceId = _resourceId
            };

            deleteResourcesProducer.PublishMessage(message);
        }
    }
}
