using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Models;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.Orchestrator
{
    public interface IOrchestratorEngine
    {
        public OrchestratorProcess GetProcess(Guid processId);
        public void DeleteProcess(Guid processId);
        public void StartGetOrganizationProcess(Guid ticketId, Guid? organizationId);
        public void StartGetRepositoriesProcess(Guid ticketId, Guid organizationId, Guid? repositoryId);
        public void StartCreateRepositoryProcess(Guid ticketId, Guid organizationId, string name);
        public void StartGetResourcesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? resourceId);
        public void StartGetResourceFilesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid resourceId);
        public void StartPostResourceProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType,
            FileDTO file);
        public void StartPostOperatorProcess(Guid ticketId, Guid organizationId, Guid repositoryId, string name, string resourceType,
            FileDTO sourceCodeFile, FileDTO dockerfileFile);
        public void StartCollabHandshakeProcess(Guid ticketId, string requestedPeerDomain);
        public void StartCollabHandshakeResponseProcess(Guid senderProcessId, Identity requesterPeerIdentity);
        public void StartRegistryUpdateProcess(Guid senderProcessId, RegistryUpdateDTO registryUpdate, IdentityDTO senderIdentity);

        public void StartPostLoginRequestProcess(Guid ticketId, string UserName, string Password);

        public void StartPostRegistrationRequestProcess(Guid ticketId, string UserName, string Password, string Name, string Role);

        // Pipeline Processes
        public void StartPostPipelineProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name);
        public void StartGetPipelinesProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid? pipelineId);
        public void StartCreatePipelineExecutionProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid pipelineId);
        public void StartTransferDataActionProcess(Guid? senderProcessId, IdentityDTO orchestratorIdentity, TransferDataActionDTO data);
        public void StartSendTransferDataActionProcess(TransferDataActionDTO data);
        public void StartExecuteOperatorActionProcess(Guid? senderProcessId, IdentityDTO orchestratorIdentity, ExecuteOperatorActionDTO data);
        public void StartSendExecuteOperatorActionProcess(ExecuteOperatorActionDTO data);
        public void StartPipelineStartCommandProcess(Guid ticketId, Guid executionId);
        public void StartPostResourceFromPeerProcess(Guid senderProcessId, ResourceDTO resource, int storageMode, Guid executionId, IdentityDTO senderIdentity);
        public void StartGetPipelineExecutionStatusProcess(Guid ticketId, Guid executionId);
        public void StartDeleteResourceProcess(Guid messageTicketId, Guid messageOrganizationId, Guid messageRepositoryId, Guid messageResourceId);
        public void StartGetAllUsersProcess(Guid ticketId, string token);
    }
}
