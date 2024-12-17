using DAPM.PipelineOrchestratorMS.Api.Engine.Interfaces;
using DAPM.PipelineOrchestratorMS.Api.Models;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.Orchestrator.ProcessRequests;
using RabbitMQLibrary.Messages.PipelineOrchestrator;
using RabbitMQLibrary.Messages.Repository;
using RabbitMQLibrary.Models;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace DAPM.PipelineOrchestratorMS.Api.Engine
{
    public class PipelineOrchestrationEngine : IPipelineOrchestrationEngine
    {

        private readonly ILogger<IPipelineOrchestrationEngine> _logger;
        private IServiceProvider _serviceProvider;
        private Dictionary<Guid, IPipelineExecution> _pipelineExecutions;

        IQueueProducer<PostPipelineExecutionToRepoMessage> _queueProducer;
        IQueueProducer<GetPipelineExecutionsFromRepoMessage> _getPipelineExecutionsFromRepoMessageProducer;

        public PipelineOrchestrationEngine(ILogger<IPipelineOrchestrationEngine> logger, IServiceProvider serviceProvider)
        {
            _pipelineExecutions = new Dictionary<Guid, IPipelineExecution>();
            _logger = logger;
            _serviceProvider = serviceProvider;
            _queueProducer = serviceProvider.GetRequiredService<IQueueProducer<PostPipelineExecutionToRepoMessage>>();
            _getPipelineExecutionsFromRepoMessageProducer = serviceProvider.GetRequiredService<IQueueProducer<GetPipelineExecutionsFromRepoMessage>>();
        }

        public Guid CreatePipelineExecutionInstance(PipelineDTO pipeline, Guid processId)
        {
            // send a message to the DAPM.RespositoryMS.API to create and store the pipeline execution instance sending it with a message

            var message = new PostPipelineExecutionToRepoMessage()
            {
                ProcessId = processId,
                TimeToLive = TimeSpan.FromMinutes(1),
                PipelineId = pipeline.Id,
                RepositoryId = pipeline.RepositoryId,
                OrganizationId = pipeline.OrganizationId
            };

            // Send the message to the DAPM.RepositoryMS.API
            _queueProducer.PublishMessage(message);
            
            _logger.LogInformation($"A new execution instance has been created");
            
            return processId;
        }

        public void ProcessActionResult(ActionResultDTO actionResultDto)
        {
            _logger.LogInformation($"Processing action result for pipeline execution {actionResultDto.ExecutionId}");
            try {
                var execution = GetPipelineExecution(actionResultDto.ExecutionId);
                execution.ProcessActionResult(actionResultDto);
            } catch (KeyNotFoundException) {
                _logger.LogError("Execution id was not available.");
                IEnumerable<Guid> guids = _pipelineExecutions.Keys;
                foreach (var guid in guids) {
                    _logger.LogInformation($"{guid.ToString()} is an available guid.");
                }
            }
        }


        public void ExecutePipelineStartCommand(Guid processId,Guid executionId)
        {
            var message = new GetPipelineExecutionsFromRepoMessage()
            {
                ProcessId = processId,
                ExecutionId = executionId,
                TimeToLive = TimeSpan.FromMinutes(1),
            };

            _getPipelineExecutionsFromRepoMessageProducer.PublishMessage(message);
        }

        public void OnPipelineExecutionRetrieved(PipelineDTO pipeline, RabbitMQLibrary.Models.PipelineExecution ex)
        {
            PipelineExecution execution = new PipelineExecution(ex.ExecutionId, pipeline.Pipeline, _serviceProvider);

            _pipelineExecutions.Add(ex.ExecutionId, execution);

            _logger.LogInformation($"An execution with id {ex.ExecutionId} has been instantiated and added into memory storage of executions.");

            try
            {
                _logger.LogInformation($"Starting execution for pipeline with ExecutionId: {ex.ExecutionId}");

                execution.StartExecution(ex.ExecutionId);

                _logger.LogInformation($"Pipeline execution with ExecutionId: {ex.ExecutionId} is running.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while executing the pipeline with ExecutionId: {ex.ExecutionId}");
                throw;
            }

        }


        private IPipelineExecution GetPipelineExecution(Guid executionId)
        {
            return _pipelineExecutions[executionId];
        }

        public PipelineExecutionStatus GetPipelineExecutionStatus(Guid executionId)
        {
            return _pipelineExecutions[executionId].GetStatus();
        }
    }
}
