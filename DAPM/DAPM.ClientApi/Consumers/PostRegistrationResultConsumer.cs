using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

namespace DAPM.ClientApi.Consumers
{
    public class PostRegistrationResultConsumer : IQueueConsumer<PostRegistrationProcessResult>
    {
        private ILogger<PostItemResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public PostRegistrationResultConsumer(ILogger<PostItemResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(PostRegistrationProcessResult message)
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
            //result["message"] = message.Message;  

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}