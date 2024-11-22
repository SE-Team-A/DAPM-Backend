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

        public PipelineOrchestrationEngine(ILogger<IPipelineOrchestrationEngine> logger, IServiceProvider serviceProvider, IQueueProducer<PostPipelineExecutionToRepoMessage> queueProducer)
        {
            _pipelineExecutions = new Dictionary<Guid, IPipelineExecution>();
            _logger = logger;
            _serviceProvider = serviceProvider;
            _queueProducer = queueProducer;
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
            var execution = GetPipelineExecution(actionResultDto.ExecutionId);
            execution.ProcessActionResult(actionResultDto);
        }


        public void ExecutePipelineStartCommand(Guid executionId)
        {
            var execution = GetPipelineExecution(executionId);
            
            try{
                _logger.LogInformation($"Starting execution for pipeline with ExecutionId: {executionId}");
            
                execution.StartExecution(executionId);

                _logger.LogInformation($"Pipeline execution with ExecutionId: {executionId} is running.");
            }
            catch(Exception e){
                _logger.LogError(e, $"An error occurred while executing the pipeline with ExecutionId: {executionId}");
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
