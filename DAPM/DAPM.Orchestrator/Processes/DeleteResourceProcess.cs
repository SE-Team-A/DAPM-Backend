using OpenTelemetry.Resources;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;
using System.Runtime.CompilerServices;
/// <author>Ayat Al Rifai</author>

namespace DAPM.Orchestrator.Processes
{
    public class DeleteResourceProcess : OrchestratorProcess
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

        public override void OnDeleteResourcesFromRepoResult(DeleteResourceFromRepoResultMessage message)
        {
            var deleteResourceFromRegistryMessageProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<DeleteResourceFromRegistryMessage>>();
            var processResultMessage = new DeleteResourceFromRegistryMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ResourceId = _resourceId,
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId

            };

            deleteResourceFromRegistryMessageProducer.PublishMessage(processResultMessage);

        }
        public override void OnDeleteResourceFromRegistryResult (DeleteResourceFromRegistryResultMessage message){
        
        var getOrganizationsProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<GetOrganizationsMessage>>();

          var getOrganizationsMessage = new GetOrganizationsMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = null
            };

            getOrganizationsProducer.PublishMessage(getOrganizationsMessage);

        }

        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {

            var targetOrganizations = message.Organizations;
            var resourcesList = new List<ResourceDTO>() {};

            SendRegistryUpdates(targetOrganizations,
                Enumerable.Empty<OrganizationDTO>(),
                Enumerable.Empty<RepositoryDTO>(), 
                Enumerable.Empty<ResourceDTO>(), 
                Enumerable.Empty<PipelineDTO>());


        }
        
        private void SendRegistryUpdates(IEnumerable<OrganizationDTO> targetOrganizations, IEnumerable<OrganizationDTO> organizations,
            IEnumerable<RepositoryDTO> repositories, IEnumerable<ResourceDTO> resources, IEnumerable<PipelineDTO> pipelines)
        {

            var sendRegistryUpdateProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<SendRegistryUpdateMessage>>();
            var identityDTO = new IdentityDTO()
            {
                Domain = _localPeerIdentity.Domain,
                Id = _localPeerIdentity.Id,
                Name = _localPeerIdentity.Name,
            };

            var registryUpdate = new RegistryUpdateDTO()
            {
                Organizations = organizations,
                Repositories = repositories,
                Pipelines = pipelines,
                Resources = resources,
            };


            var registryUpdateMessages = new List<SendRegistryUpdateMessage>();

            foreach (var organization in targetOrganizations)
            {

                if (organization.Id == _localPeerIdentity.Id)
                    continue;

                var domain = organization.Domain;
                var registryUpdateMessage = new SendRegistryUpdateMessage()
                {
                    TargetPeerDomain = domain,
                    SenderPeerIdentity = identityDTO,
                    SenderProcessId = _processId,
                    TimeToLive = TimeSpan.FromMinutes(1),
                    RegistryUpdate = registryUpdate,
                    IsPartOfHandshake = false,
                };

                registryUpdateMessages.Add(registryUpdateMessage);
            //    _isRegistryUpdateCompleted[organization.Id] = false;
            //    _registryUpdatesNotCompletedCounter++;
            }

            if (registryUpdateMessages.Count() == 0)
            {
                FinishProcess();
            }
            else
            {
                foreach (var message in registryUpdateMessages)
                    sendRegistryUpdateProducer.PublishMessage(message);
            }

        }


         private void FinishProcess()
        {
        var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var itemsIds = new ItemIds()
            {
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId
            };

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemIds = itemsIds,
                ItemType = "Resource",
                Message = "The item was deleted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();

    }
    }
}
