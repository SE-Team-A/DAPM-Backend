﻿using DAPM.Orchestrator.Processes;
using DAPM.Orchestrator.Processes.PipelineActions;
using DAPM.Orchestrator.Processes.PipelineCommands;
using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Models;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
/// <author>Tamás Drabos</author>
namespace DAPM.Orchestrator
{
    public class OrchestratorEngine : IOrchestratorEngine
    {
        private ILogger<OrchestratorEngine> _logger;
        private Dictionary<Guid, OrchestratorProcess> _processes;
        private IServiceProvider _serviceProvider;

        public OrchestratorEngine(IServiceProvider serviceProvider, ILogger<OrchestratorEngine> logger)
        {
            _processes = new Dictionary<Guid, OrchestratorProcess>();
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void DeleteProcess(Guid processId)
        {
            _logger.LogInformation("ORCHESTRATOR ENGINE removing process with id " + processId.ToString());
            _processes.Remove(processId);
        }

        public OrchestratorProcess GetProcess(Guid processId)
        {
            return _processes[processId];
        }


        #region PROCESSES TRIGGERED FROM CLIENT API

        public void StartCollabHandshakeProcess(Guid apiTicketId, string requestedPeerDomain)
        {
            var processId = Guid.NewGuid();
            var collabHandshakeProcess = new CollabHandshakeProcess(this, _serviceProvider, apiTicketId, processId, requestedPeerDomain);
            _processes[processId] = collabHandshakeProcess;
            collabHandshakeProcess.StartProcess();
        }

        public void StartGetPipelineExecutionsProcess(Guid ticketId, Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            var processId = Guid.NewGuid();
            var process = new GetPipelineExecutionsProcess(this, _serviceProvider, ticketId, processId, organizationId, pipelineId);
            _processes[processId] = process;
            process.StartProcess();
        }

        public void StartCreatePipelineExecutionProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, Guid pipelineId)
        {
            var processId = Guid.NewGuid();
            var createPipelineExecutionProcess = new CreatePipelineExecutionProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, pipelineId);
            _processes[processId] = createPipelineExecutionProcess;
            createPipelineExecutionProcess.StartProcess();
        }

        public void StartCreateRepositoryProcess(Guid apiTicketId, Guid organizationId, string name)
        {
            var processId = Guid.NewGuid();
            var createRepositoryProcess = new CreateRepositoryProcess(this, _serviceProvider, apiTicketId, processId, organizationId, name);
            _processes[processId] = createRepositoryProcess;
            createRepositoryProcess.StartProcess();
        }

        public void StartGetOrganizationProcess(Guid apiTicketId, Guid? organizationId)
        {
            var processId = Guid.NewGuid();
            var getOrganizationProcess = new GetOrganizationsProcess(this, _serviceProvider, apiTicketId, processId, organizationId);
            _processes[processId] = getOrganizationProcess;
            getOrganizationProcess.StartProcess();
        }

        public void StartGetPipelinesProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, Guid? pipelineId)
        {
            var processId = Guid.NewGuid();
            var getPipelinesProcess = new GetPipelinesProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, pipelineId);
            _processes[processId] = getPipelinesProcess;
            getPipelinesProcess.StartProcess();
        }

        public void StartGetRepositoriesProcess(Guid apiTicketId, Guid organizationId, Guid? repositoryId)
        {
            var processId = Guid.NewGuid();
            var getRepositoriesProcess = new GetRepositoriesProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId);
            _processes[processId] = getRepositoriesProcess;
            getRepositoriesProcess.StartProcess();
        }

        public void StartGetResourceFilesProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, Guid resourceId)
        {
            var processId = Guid.NewGuid();
            var getResourceFilesProcess = new GetResourceFilesProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, resourceId);
            _processes[processId] = getResourceFilesProcess;
            getResourceFilesProcess.StartProcess();
        }

        public void StartGetResourcesProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, Guid? resourceId)
        {
            var processId = Guid.NewGuid();
            var getResourcesProcess = new GetResourcesProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, resourceId);
            _processes[processId] = getResourcesProcess;
            getResourcesProcess.StartProcess();
        }

        public void StartPipelineStartCommandProcess(Guid apiTicketId, Guid executionId)
        {
            var processId = Guid.NewGuid();
            var pipelineStartCommandProcess = new PipelineStartCommandProcess(this, _serviceProvider, apiTicketId, processId, executionId);
            _processes[processId] = pipelineStartCommandProcess;
            pipelineStartCommandProcess.StartProcess();
        }

        public void StartPostPipelineProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name)
        {
            var processId = Guid.NewGuid();
            var postPipelineProcess = new PostPipelineProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, pipeline, name);
            _processes[processId] = postPipelineProcess;
            postPipelineProcess.StartProcess();
        }
        public void StartEditPipelineProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, Pipeline pipeline, string name, Guid pipelineId)
        {
            var processId = Guid.NewGuid();
            var editPipelineProcess = new EditPipelineProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, pipeline, name, pipelineId);
            _processes[processId] = editPipelineProcess;
            editPipelineProcess.StartProcess();
        }


        public void StartPostResourceProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, string name, string resourceType, FileDTO file)
        {
            var processId = Guid.NewGuid();
            var postResourceProcess = new PostResourceProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, name, resourceType, file);
            _processes[processId] = postResourceProcess;
            postResourceProcess.StartProcess();
        }

        public void StartPostOperatorProcess(Guid apiTicketId, Guid organizationId, Guid repositoryId, string name, string resourceType, FileDTO sourceCodeFile,
            FileDTO dockerfileFile)
        {
            var processId = Guid.NewGuid();
            var postOperatorProcess = new PostOperatorProcess(this, _serviceProvider, apiTicketId, processId, organizationId, repositoryId, name, resourceType, sourceCodeFile,
                dockerfileFile);
            _processes[processId] = postOperatorProcess;
            postOperatorProcess.StartProcess();
        }

        public void StartGetPipelineExecutionStatusProcess(Guid ticketId, Guid executionId)
        {
            var processId = Guid.NewGuid();
            var getPipelineExecutionStatusProcess = new GetPipelineExecutionStatusProcess(this, _serviceProvider, ticketId, processId, executionId);
            _processes[processId] = getPipelineExecutionStatusProcess;
            getPipelineExecutionStatusProcess.StartProcess();
        }

        public void StartDeleteResourceProcess(Guid messageTicketId, Guid messageOrganizationId, Guid messageRepositoryId,
            Guid messageResourceId)
        {
            var processId = Guid.NewGuid();
            var postResourceProcess = new DeleteResourceProcess(this, _serviceProvider, processId, messageOrganizationId, messageRepositoryId, messageTicketId, messageResourceId);
            _processes[processId] = postResourceProcess;
            postResourceProcess.StartProcess();
            //postResourceProcess.OnDeleteResourcesFromRegistryResult();
        }

        public void StartDeletePipelineProcess(Guid messageTicketId, Guid messageOrganizationId, Guid messageRepositoryId, Guid messagePipelineId)
        {
            var processId = Guid.NewGuid();
            var postPipelineProcess = new DeletePipelineProcess(this, _serviceProvider, processId, messageOrganizationId, messageRepositoryId, messageTicketId, messagePipelineId);
            _processes[processId] = postPipelineProcess;
            postPipelineProcess.StartProcess();
        }


        #endregion

        #region PROCESSES TRIGGERED BY SYSTEM


        public void StartCollabHandshakeResponseProcess(Guid senderProcessId, Identity requesterPeerIdentity)
        {
            var registerPeerProcess = new CollabHandshakeResponseProcess(this, _serviceProvider, senderProcessId, requesterPeerIdentity);
            _processes[senderProcessId] = registerPeerProcess;
            registerPeerProcess.StartProcess();
        }



        public void StartExecuteOperatorActionProcess(Guid? senderProcessId, IdentityDTO orchestratorIdentity, ExecuteOperatorActionDTO data)
        {
            var processId = Guid.NewGuid();
            var executeOperatorActionProcess = new ExecuteOperatorActionProcess(this, _serviceProvider, processId, senderProcessId, data, orchestratorIdentity);
            _processes[processId] = executeOperatorActionProcess;
            executeOperatorActionProcess.StartProcess();
        }



        public void StartTransferDataActionProcess(Guid? senderProcessId, IdentityDTO orchestratorIdentity, TransferDataActionDTO data)
        {
            var processId = Guid.NewGuid();
            var transferDataActionProcess = new TransferDataActionProcess(this, _serviceProvider, processId, senderProcessId, data, orchestratorIdentity);
            _processes[processId] = transferDataActionProcess;
            transferDataActionProcess.StartProcess();
        }

        public void StartRegistryUpdateProcess(Guid senderProcessId, RegistryUpdateDTO registryUpdate, IdentityDTO senderIdentity)
        {
            var processId = Guid.NewGuid();
            var registryUpdateProcess = new RegistryUpdateProcess(this, _serviceProvider, processId, senderProcessId, registryUpdate, senderIdentity);
            _processes[processId] = registryUpdateProcess;
            registryUpdateProcess.StartProcess();
        }

        public void StartPostResourceFromPeerProcess(Guid senderProcessId, ResourceDTO resource, int storageMode, Guid executionId, IdentityDTO senderIdentity)
        {
            var processId = Guid.NewGuid();
            var postResourceProcess = new PostResourceFromPeerProcess(this, _serviceProvider, processId, senderProcessId, resource, storageMode, executionId, senderIdentity);
            _processes[processId] = postResourceProcess;
            postResourceProcess.StartProcess();
        }

        public void StartSendTransferDataActionProcess(TransferDataActionDTO data)
        {
            var processId = Guid.NewGuid();
            var sendTransferDataActionProcess = new SendTransferDataActionProcess(this, _serviceProvider, processId, data);
            _processes[processId] = sendTransferDataActionProcess;
            sendTransferDataActionProcess.StartProcess();
        }

        public void StartSendExecuteOperatorActionProcess(ExecuteOperatorActionDTO data)
        {
            var processId = Guid.NewGuid();
            var sendExecuteOperatorActionProcess = new SendExecuteOperatorActionProcess(this, _serviceProvider, processId, data);
            _processes[processId] = sendExecuteOperatorActionProcess;
            sendExecuteOperatorActionProcess.StartProcess();
        }

        public void StartPostLoginRequestProcess(Guid ticketId, string UserName, string Password)
        {
            var processId = Guid.NewGuid();
            var postLogingRequestProcess = new PostLoginRequestProcess(this, _serviceProvider, ticketId, processId, UserName, Password);
            _processes[processId] = postLogingRequestProcess;
            postLogingRequestProcess.StartProcess();
        }

        public void StartPostRegistrationRequestProcess(Guid ticketId, string UserName, string Password, string Name, string Role)
        {
            var processId = Guid.NewGuid();
            var postRegistrationRequestProcess = new PostRegistrationRequestProcess(this, _serviceProvider, ticketId, processId, UserName, Password, Name, Role);
            _processes[processId] = postRegistrationRequestProcess;
            postRegistrationRequestProcess.StartProcess();
        }

        public void StartGetAllUsersProcess(Guid ticketId, string Token)
        {
            var processId = Guid.NewGuid();
            var getAllUsersRequestProcess = new GetAllUsersRequestProcess(this, _serviceProvider,ticketId, processId, Token);
            _processes[processId] = getAllUsersRequestProcess;
            getAllUsersRequestProcess.StartProcess();
        }
        
        public void StartPostUserRoleProcess(Guid ticketId, string RequestToken, Guid UserId, string RoleName)
        {
            var processId = Guid.NewGuid();
            var postUserRoleProcess = new PostUserRoleProcess(this, _serviceProvider,ticketId, processId,RequestToken, UserId, RoleName);
            _processes[processId] = postUserRoleProcess;
            postUserRoleProcess.StartProcess();
        }

        public void StartDeleteUserProcess(Guid ticketId, string RequestToken, Guid UserId)
        {
            var processId = Guid.NewGuid();
            var deleteUserProcess = new DeleteUserProcess(this, _serviceProvider, ticketId, processId, RequestToken, UserId);
            _processes[processId] = deleteUserProcess;
            deleteUserProcess.StartProcess();
        }


        #endregion
    }
}
