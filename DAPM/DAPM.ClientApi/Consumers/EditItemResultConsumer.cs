using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;
/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ClientApi.Consumers
{
    public class EditItemResultConsumer : IQueueConsumer<EditItemProcessResult>
    {
        private ILogger<PostItemResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public EditItemResultConsumer(ILogger<PostItemResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(EditItemProcessResult message)
        {
            _logger.LogInformation("EditItemResultMessage received");


            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            JToken idsJSON = JToken.FromObject(message.ItemIds, serializer);

            //Serialization
            result["itemIds"] = idsJSON;
            result["itemType"] = message.ItemType;
            result["succeeded"] = message.Succeeded;
            result["message"] = message.Message;  

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}
