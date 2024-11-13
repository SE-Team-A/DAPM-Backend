using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Consumers
{
    public class GetPipelineExecutionsProcessResultConsumer: IQueueConsumer<GetPipelineExecutionsProcessResult>
    {
        private ILogger<GetPipelinesProcessResultConsumer> _logger;
        private readonly ITicketService _ticketService;

        public GetPipelineExecutionsProcessResultConsumer(ILogger<GetPipelinesProcessResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetPipelineExecutionsProcessResult message)
        {
            _logger.LogInformation("GetPipelineExecutionsProcessResult received");


            IEnumerable<PipelineExecution> pipelineExecutions = message.PipelineExecutions;

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });


            //Serialization
            JToken pipelinesJSON = JToken.FromObject(pipelineExecutions, serializer);
            result["pipelineExecutions"] = pipelinesJSON;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
