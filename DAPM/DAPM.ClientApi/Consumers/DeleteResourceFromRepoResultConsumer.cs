using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;
// AYAT AL RIFAI
namespace DAPM.ClientApi.Consumers
{
    public class DeleteResourceFromRepoResultConsumer : IQueueConsumer<DeleteResourceFromRepoResult>
    {
        private ILogger<DeleteResourceFromRepoResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public DeleteResourceFromRepoResultConsumer(ILogger<DeleteResourceFromRepoResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(DeleteResourceFromRepoResult message)
        {
            _logger.LogInformation("DeleteResourceFromRepoResult received");

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            //JToken idsJSON = JToken.FromObject(message., serializer);

            //Serialization
            //result["itemIds"] = idsJSON;
            //result["itemType"] = message.ItemType;
            result["resourceId"] = message.resourceId;
            //result["message"] = message.Message;  

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);


            return Task.CompletedTask;

        }

    }
}
