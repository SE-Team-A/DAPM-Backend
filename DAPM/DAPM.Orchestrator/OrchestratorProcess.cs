﻿using DAPM.Orchestrator.Consumers.StartProcessConsumers;
using DAPM.Orchestrator.Processes;
using DAPM.Orchestrator.Services;
using DAPM.Orchestrator.Services.Models;
using RabbitMQLibrary.Messages.Orchestrator.Other;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromOperator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPeerApi;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromPipelineOrchestrator;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
/// <author>Tamas Drabos</author>
namespace DAPM.Orchestrator
{
    public abstract class OrchestratorProcess : IOrchestratorProcess
    {
        private IServiceProvider _serviceProvider;
        protected IServiceScope _serviceScope;
        protected OrchestratorEngine _engine;
        protected Identity _localPeerIdentity;
        protected Guid _processId;

        public OrchestratorProcess(OrchestratorEngine engine, IServiceProvider serviceProvider, Guid processId)
        {
            _engine = engine;
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.CreateScope();
            _processId = processId;

            var identityService = _serviceScope.ServiceProvider.GetRequiredService<IIdentityService>();
            _localPeerIdentity = identityService.GetIdentity();
        }

        public abstract void StartProcess();
        public virtual void EndProcess()
        {
            _engine.DeleteProcess(_processId);
        }


        public virtual void OnPostRepoToRegistryResult(PostRepoToRegistryResultMessage message)
        {
            return;
        }

        public virtual void OnAddResourceToRegistryResult()
        {
            return;
        }

        public virtual void OnGetOrganizationsFromRegistryResult(GetOrganizationsResultMessage message)
        {
            return;
        }

        public virtual void OnGetRepositoriesFromRegistryResult(GetRepositoriesResultMessage message)
        {
            return;
        }

        public virtual void OnGetResourcesFromRegistryResult(GetResourcesResultMessage message)
        {
            return;
        }

        public virtual void OnGetPipelinesFromRegistryResult()
        {
            return;
        }

        public virtual void OnPostPipelineToRepoResult(PostPipelineToRepoResultMessage message)
        {
            return;
        }
        public virtual void OnEditPipelineToRepoResult(EditPipelineInRepoResultMessage message)
        {
            return;
        }

        public virtual void OnEditPipelineToRegistryResult(EditPipelineToRegistryResultMessage message)
        {
            return;
        }


        public virtual void OnCreateRepoInRepoResult(PostRepoToRepoResultMessage message)
        {
            return;
        }

        public virtual void OnPostResourceToRepoResult(PostResourceToRepoResultMessage message)
        {
            return;
        }

        public virtual void OnPostResourceToRegistryResult(PostResourceToRegistryResultMessage message)
        {
            return;
        }

        public virtual void OnGetPipelinesFromRepoResult(GetPipelinesFromRepoResultMessage message)
        {
            return;
        }

        public virtual void OnGetPipelineExecutionsFromRepoResult(GetPipelineExecutionsFromRepoResultMessage message)
        {
            return;
        }

        public virtual void OnGetPipelinesFromRegistryResult(GetPipelinesResultMessage message)
        {
            return;
        }

        public virtual void OnPostPipelineToRegistryResult(PostPipelineToRegistryResultMessage message)
        {
            return;
        }

        public virtual void OnGetResourceFilesFromRepoResult(GetResourceFilesFromRepoResultMessage message)
        {
            return;
        }
        public virtual void OnDeleteResourcesFromRepoResult(DeleteResourceFromRepoResultMessage message)
        {
            return;
        }

        public virtual void OnHandshakeRequestResponse(HandshakeRequestResponseMessage message)
        {
            return;
        }

        public virtual void OnRegistryUpdate(RegistryUpdateMessage message)
        {
            return;
        }

        public virtual void OnApplyRegistryUpdateResult(ApplyRegistryUpdateResult message)
        {
            return;
        }

        public virtual void OnGetEntriesFromOrgResult(GetEntriesFromOrgResult message)
        {
            return;
        }

        public virtual void OnRegistryUpdateAck(RegistryUpdateAckMessage message)
        {
            return;
        }

        public virtual void OnCreatePipelineExecutionResult(CreatePipelineExecutionResultMessage message)
        {
            return;
        }

        public virtual void OnCommandEnqueued(CommandEnqueuedMessage message)
        {
            return;
        }

        public virtual void OnGetResourceFilesFromOperatorResult(GetExecutionOutputResultMessage message)
        {
            return;
        }

        public virtual void OnPostResourceToOperatorResult(PostInputResourceResultMessage message)
        {
            return;
        }

        public virtual void OnSendResourceToPeerResult(SendResourceToPeerResultMessage message)
        {
            return;
        }

        public virtual void OnExecuteOperatorResult(ExecuteOperatorResultMessage message)
        {
            return;
        }

        public virtual void OnGetOperatorFilesFromRepoResult(GetOperatorFilesFromRepoResultMessage message)
        {
            return;
        }

        public virtual void OnActionResultFromPeer(ActionResultReceivedMessage message)
        {
            return;
        }

        public virtual void OnGetPipelineExecutionStatusResult(GetPipelineExecutionStatusResultMessage message)
        {
            return;
        }

        public virtual void OnPostLoginResult(PostLoginResultMessage message)
        {
            return;
        }

        public virtual void OnPostRegistrationResult(PostRegistrationResultMessage message)
        {
            return;
        }

        public virtual void OnDeleteResourceFromRegistryResult(DeleteResourceFromRegistryResultMessage message)
        {
            return;
        }

        public virtual void OnDeleteRepositoryPipelineResult(DeleteRepositoryPipelineResultMessage message)
        {
            return;
        }

        public virtual void OnDeleteRegistryPipelineResult(DeleteRegistryPipelineResultMessage message)
        {
            return;
        }
        public virtual void OnGetAllUsersResult(GetAllUsersResultMessage message)
        {
            return;
        }

        public virtual void OnPostUserRoleResult(PostUserRoleResultMessage message)
        {
            return;
        }

        public virtual void OnDeleteUserResult(DeleteUserResultMessage message)
        {
            return;
        }
    }
}
