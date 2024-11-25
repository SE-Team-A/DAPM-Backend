using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

/// <author>Raihanullah Mehran</author>
namespace DAPM.ClientApi.Consumers
{
    public class DeleteRepositoryPipelineConsumer : IQueueConsumer<DeleteRepositoryPipelineResult>
    {
        private ILogger<DeleteResourceFromRepoResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public DeleteRepositoryPipelineConsumer(ILogger<DeleteResourceFromRepoResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(DeleteRepositoryPipelineResult message)
        {
            _logger.LogInformation("DeletePipelineProcessResult received");

            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            result["pipelineId"] = message.PipelineId;

            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}