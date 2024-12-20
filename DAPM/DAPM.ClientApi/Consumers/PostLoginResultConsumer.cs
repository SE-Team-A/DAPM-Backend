using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>

namespace DAPM.ClientApi.Consumers
{
    public class PostLoginResultConsumer : IQueueConsumer<PostLoginProcessResult>
    {
        private ILogger<PostItemResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public PostLoginResultConsumer(ILogger<PostItemResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(PostLoginProcessResult message)
        {
            _logger.LogInformation("CreateNewItemResultMessage received");


            // Objects used for serialization
            JToken result = new JObject();
            // JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            // JToken loginResult = JToken.FromObject(result, serializer);

            //Serialization
            //result["itemIds"] = idsJSON;
            //result["itemType"] = message.ItemType;
            result["succeeded"] = message.Succeeded;
            result["token"] = message.Token;
            //result["message"] = message.Message;  

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}