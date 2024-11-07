using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;
using RabbitMQLibrary.Messages.PeerApi;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Messages.ResourceRegistry;
using RabbitMQLibrary.Models;

namespace DAPM.Orchestrator.Processes
{
    public class EditPipelineProcess : OrchestratorProcess
    {

        private Guid _organizationId;
        private Guid _repositoryId;
        private string _pipelineName;
        private Pipeline _pipeline;

        private PipelineDTO _createdPipeline;
        private Guid _pipelineId; 


        private Dictionary<Guid, bool> _isRegistryUpdateCompleted;
        private int _registryUpdatesNotCompletedCounter;

        private Guid _ticketId;

        public EditPipelineProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid ticketId, Guid processId,
            Guid organizationId, Guid repositoryId, Pipeline pipeline, string name, Guid pipelineId)
            : base(engine, serviceProvider, processId)
        {
            _organizationId = organizationId;
            _repositoryId = repositoryId;
            _pipeline = pipeline;
            _pipelineName = name;
            _pipelineId = pipelineId;

            _registryUpdatesNotCompletedCounter = 0;
            _isRegistryUpdateCompleted = new Dictionary<Guid, bool>();

            _ticketId = ticketId;
        }   
        public override void StartProcess()
        {
            var EditPipelineInRepoProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<EditPipelineInRepoMessage>>();

            var message = new EditPipelineInRepoMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                OrganizationId = _organizationId,
                RepositoryId = _repositoryId,
                Name = _pipelineName,
                Pipeline = _pipeline,
                PipelineId = _pipelineId,
            };

            EditPipelineInRepoProducer.PublishMessage(message);
        }
        

        public override void OnEditPipelineToRepoResult(EditPipelineInRepoResultMessage message)
        {
            _createdPipeline = message.Pipeline;
            var editPipelineToRegistryProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<EditPipelineToRegistryMessage>>();

            message.Pipeline.OrganizationId = _organizationId;

            var editPipelineToRegistryMessage = new EditPipelineToRegistryMessage()
            {
                ProcessId = _processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                Pipeline = message.Pipeline,
            };

            editPipelineToRegistryProducer.PublishMessage(editPipelineToRegistryMessage);

        }

        public override void OnEditPipelineToRegistryResult(EditPipelineToRegistryResultMessage message)
        {
            var editItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<EditItemProcessResult>>();

            var itemsIds = new ItemIds()
            {
                OrganizationId = _createdPipeline.OrganizationId,
                RepositoryId = _createdPipeline.RepositoryId,
                PipelineId = _createdPipeline.Id,
            };

            var editItemProcessResultMessage = new EditItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemIds = itemsIds,
                ItemType = "Pipeline",
                Message = "The item was edited successfully",
                Succeeded = true
            };

            editItemProcessResultProducer.PublishMessage(editItemProcessResultMessage);

            EndProcess();
        }
/*
        public override void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {

            var targetOrganizations = message.Organizations;
            var pipelinesList = new List<PipelineDTO>() { _createdPipeline };


            SendRegistryUpdates(targetOrganizations,
                Enumerable.Empty<OrganizationDTO>(),
                Enumerable.Empty<RepositoryDTO>(),
                Enumerable.Empty<ResourceDTO>(),
                pipelinesList);


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
                _isRegistryUpdateCompleted[organization.Id] = false;
                _registryUpdatesNotCompletedCounter++;
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

        public override void OnRegistryUpdateAck(RegistryUpdateAckMessage message)
        {
            var organizationId = message.PeerSenderIdentity.Id;
            if (message.RegistryUpdateAck.IsCompleted)
            {
                _isRegistryUpdateCompleted[(Guid)organizationId] = true;
                _registryUpdatesNotCompletedCounter--;
            }

            if (_registryUpdatesNotCompletedCounter == 0)
            {
                FinishProcess();
            }
        }


        private void FinishProcess()
        {
            var postItemProcessResultProducer = _serviceScope.ServiceProvider.GetRequiredService<IQueueProducer<PostItemProcessResult>>();

            var itemsIds = new ItemIds()
            {
                OrganizationId = _createdPipeline.OrganizationId,
                RepositoryId = _createdPipeline.RepositoryId,
                PipelineId = _createdPipeline.Id,
            };

            var postItemProcessResultMessage = new PostItemProcessResult()
            {
                TicketId = _ticketId,
                TimeToLive = TimeSpan.FromMinutes(1),
                ItemIds = itemsIds,
                ItemType = "Pipeline",
                Message = "The item was posted successfully",
                Succeeded = true
            };

            postItemProcessResultProducer.PublishMessage(postItemProcessResultMessage);

            EndProcess();
        }
*/
    }
    
}
